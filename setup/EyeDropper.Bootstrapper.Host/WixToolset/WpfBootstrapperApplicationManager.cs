using System.IO;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;
using EyeDropper.Bootstrapper.Core;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.Core.Helpers;
using WixToolset.BootstrapperApplicationApi;
using ErrorEventArgs = WixToolset.BootstrapperApplicationApi.ErrorEventArgs;

namespace EyeDropper.Bootstrapper.Host.WixToolset;

/// <summary>
/// Manages the bootstrapper application lifecycle and orchestrates install, uninstall,
/// repair, and layout operations for the bundle.
/// </summary>
public class WpfBootstrapperApplicationManager : IWpfBootstrapperApplicationManager, IDisposable
{
    #region Fields/Consts

    private const int UserCancelledErrorCode = 1223;
    private const string EyeDropperPackageId = "EyeDropper";

    private readonly Dictionary<string, int> _packageOrder = [];
    private readonly Dispatcher _dispatcher;
    private readonly WixStringVariable _installDirVar;
    private readonly WixStringVariable _layoutDirVar;
    private readonly WixBooleanVariable _createShortcutVar;
    private readonly WixBooleanVariable _launchOnStartupVar;
    private readonly WixBooleanVariable _createStartMenuShortcutVar;
    private readonly WixStringVariable _versionVar;
    private readonly WixStringVariable _logFileVar;
    private readonly WixStringVariable _bundleNameVar;
    private readonly WixStringVariable _licenseVar;
    private readonly IWpfBootstrapperApplication _eyeDropperBA;
    private readonly object _syncRoot = new();

    private int _result;
    private int _progressPhases;
    private int _cacheProgress;
    private int _executeProgress;
    private bool _autoClose;
    private bool _disposedValue;
    private ErrorType? _errorType;
    private bool _isRetry;

    /// <inheritdoc/>
    public event EventHandler<ActionRequestedEventArgs>? OnActionRequested;

    /// <inheritdoc/>
    public event EventHandler<ActionCompletedEventArgs>? OnActionCompleted;

    /// <inheritdoc/>
    public event EventHandler<InstallationProgressEventArgs>? OnProgress;

    /// <inheritdoc/>
    public event EventHandler? OnCanceled;

    #endregion

    #region Properties

    /// <inheritdoc/>
    public LaunchAction Action => _eyeDropperBA.Command.Action;

    /// <summary>
    /// The action that the engine has planned to perform.
    /// </summary>
    public LaunchAction PlannedAction { get; private set; }

    /// <inheritdoc/>
    public Display Display => _eyeDropperBA.Command.Display;

    /// <inheritdoc/>
    public InstallationState InstallationState { get; private set; }

    /// <inheritdoc/>
    public InstallationState PreApplyState { get; private set; }

    /// <inheritdoc/>
    public DetectionState DetectState { get; private set; }

    /// <inheritdoc/>
    public UpgradeDetectionState UpgradeDetectState { get; private set; }

    /// <inheritdoc/>
    public string BundleName => _bundleNameVar.Get();

    /// <inheritdoc/>
    public string LicenseUrl => _licenseVar.Get();

    /// <inheritdoc/>
    public string Version => _versionVar.Get();

    /// <inheritdoc/>
    public string? ExistingVersion { get; private set; }

    /// <inheritdoc/>
    public string LogFilePath => _logFileVar.Get();

    /// <inheritdoc/>
    public string InstallDirectory
    {
        get => _installDirVar.Get();
        set => _installDirVar.Set(value);
    }

    /// <inheritdoc/>
    public string LayoutDirectory
    {
        get => _layoutDirVar.Get();
        private set => _layoutDirVar.Set(value);
    }

    /// <inheritdoc/>
    public bool CreateDesktopShortcut
    {
        get => _createShortcutVar.Get();
        set => _createShortcutVar.Set(value);
    }

    /// <inheritdoc/>
    public bool CreateStartMenuShortcut
    {
        get => _createStartMenuShortcutVar.Get();
        set => _createStartMenuShortcutVar.Set(value);
    }

    /// <inheritdoc/>
    public bool LaunchOnStartup
    {
        get => _launchOnStartupVar.Get();
        set => _launchOnStartupVar.Set(value);
    }

    /// <inheritdoc/>
    public bool Downgrade { get; private set; }

    /// <inheritdoc/>
    public bool Canceled { get; private set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="WpfBootstrapperApplicationManager"/> class.
    /// </summary>
    /// <param name="eyeDropperBA">The bootstrapper application engine interface.</param>
    /// <param name="dispatcher">The WPF dispatcher for UI thread invocation.</param>
    public WpfBootstrapperApplicationManager(IWpfBootstrapperApplication eyeDropperBA, Dispatcher dispatcher)
    {
        _eyeDropperBA = eyeDropperBA;
        _dispatcher = dispatcher;

        _installDirVar = new(_eyeDropperBA.Engine, BundleVar.InstallDirectory);
        _layoutDirVar = new(_eyeDropperBA.Engine, BundleVar.LayoutDirectory);
        _createShortcutVar = new(_eyeDropperBA.Engine, BundleVar.CreateDesktopShortcut);
        _launchOnStartupVar = new(_eyeDropperBA.Engine, BundleVar.LaunchOnStartup);
        _bundleNameVar = new(_eyeDropperBA.Engine, BundleVar.Name);
        _versionVar = new(_eyeDropperBA.Engine, BundleVar.Version);
        _logFileVar = new(_eyeDropperBA.Engine, BundleVar.Log);
        _licenseVar = new(_eyeDropperBA.Engine, BundleVar.License);
        _createStartMenuShortcutVar = new(_eyeDropperBA.Engine, BundleVar.CreateStartMenuShortcut);

        WireEvents();
    }

    #region Methods

    /// <summary>
    /// Starts the bootstrapper run loop, invoking detection and UI or headless execution as needed.
    /// </summary>
    /// <returns>The engine execution result code.</returns>
    public int Run()
    {
        InstallationState = InstallationState.Initializing;

        if (Display is Display.Passive or Display.Full)
        {
            var app = new App();
            _eyeDropperBA.Engine.Detect();
            app.Run();
        }
        else
        {
            _eyeDropperBA.Engine.Detect();
            Dispatcher.Run();
        }

        return _result;
    }

    /// <inheritdoc/>
    public void Install()
    {
        if (DetectState is DetectionState.Present
            && UpgradeDetectState is UpgradeDetectionState.None)
        {
            Plan(LaunchAction.Repair);
        }
        else
        {
            Plan(LaunchAction.Install);
        }
    }

    /// <inheritdoc/>
    public void Uninstall()
    {
        Plan(LaunchAction.Uninstall);
    }

    /// <inheritdoc/>
    public void Cancel(bool autoClose = false)
    {
        Canceled = true;
        _autoClose = autoClose;
        OnCanceled?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc/>
    public void Close()
    {
        _dispatcher.InvokeShutdown();
    }

    /// <inheritdoc/>
    public void CloseSplashScreen()
    {
        _eyeDropperBA.Engine.CloseSplashScreen();
    }

    /// <inheritdoc/>
    public void ResetState()
    {
        if (Canceled)
        {
            _isRetry = true;
            _autoClose = false;
            Canceled = false;
            _eyeDropperBA.Engine.Detect();
        }
    }

    /// <summary>
    /// Subscribes to all necessary bootstrapper engine events with their corresponding handlers.
    /// </summary>
    private void WireEvents()
    {
        _eyeDropperBA.DetectBegin += DetectBegin;
        _eyeDropperBA.DetectComplete += DetectComplete;
        _eyeDropperBA.DetectRelatedBundle += DetectRelatedBundle;
        _eyeDropperBA.DetectPackageComplete += DetectPackageComplete;
        _eyeDropperBA.PlanBegin += PlanBegin;
        _eyeDropperBA.PlanComplete += PlanComplete;
        _eyeDropperBA.ApplyBegin += ApplyBegin;
        _eyeDropperBA.Progress += Progress;
        _eyeDropperBA.ApplyComplete += ApplyComplete;
        _eyeDropperBA.Error += Error;
        _eyeDropperBA.ExecuteProgress += ExecuteProgress;
        _eyeDropperBA.PlannedPackage += PlannedPackage;
        _eyeDropperBA.CacheAcquireProgress += CacheAcquireProgress;
        _eyeDropperBA.CacheContainerOrPayloadVerifyProgress += CacheContainerOrPayloadVerifyProgress;
        _eyeDropperBA.CachePayloadExtractProgress += CachePayloadExtractProgress;
        _eyeDropperBA.CacheVerifyProgress += CacheVerifyProgress;
        _eyeDropperBA.CacheComplete += CacheComplete;
    }

    /// <summary>
    /// Unsubscribes from all previously wired bootstrapper engine events.
    /// </summary>
    private void UnwireEvents()
    {
        _eyeDropperBA.DetectBegin -= DetectBegin;
        _eyeDropperBA.DetectComplete -= DetectComplete;
        _eyeDropperBA.DetectRelatedBundle -= DetectRelatedBundle;
        _eyeDropperBA.DetectPackageComplete -= DetectPackageComplete;
        _eyeDropperBA.PlanBegin -= PlanBegin;
        _eyeDropperBA.PlanComplete -= PlanComplete;
        _eyeDropperBA.ApplyBegin -= ApplyBegin;
        _eyeDropperBA.Progress -= Progress;
        _eyeDropperBA.ApplyComplete -= ApplyComplete;
        _eyeDropperBA.Error -= Error;
        _eyeDropperBA.ExecuteProgress -= ExecuteProgress;
        _eyeDropperBA.PlannedPackage -= PlannedPackage;
        _eyeDropperBA.CacheAcquireProgress -= CacheAcquireProgress;
        _eyeDropperBA.CacheContainerOrPayloadVerifyProgress -= CacheContainerOrPayloadVerifyProgress;
        _eyeDropperBA.CachePayloadExtractProgress -= CachePayloadExtractProgress;
        _eyeDropperBA.CacheVerifyProgress -= CacheVerifyProgress;
        _eyeDropperBA.CacheComplete -= CacheComplete;
    }

    /// <summary>
    /// Parses the command line arguments passed to the bootstrapper and updates corresponding bundle variables.
    /// </summary>
    private void ParseCommandLine()
    {
        var args = BootstrapperCommand.ParseCommandLineToArgs(_eyeDropperBA.Command.CommandLine);
        foreach (var arg in args)
        {
            var parts = arg.Split('=', 2);
            var key = parts[0];
            var value = parts.Length > 1 ? parts[1] : string.Empty;

            if (key.Equals(BundleVar.InstallDirectory, StringComparison.InvariantCultureIgnoreCase))
            {
                InstallDirectory = Path.Combine(Environment.CurrentDirectory, value);
            }
            else if (key.Equals(BundleVar.CreateDesktopShortcut, StringComparison.InvariantCultureIgnoreCase))
            {
                if (bool.TryParse(value, out var b)) CreateDesktopShortcut = b;
                else if (value == "1") CreateDesktopShortcut = true;
                else if (value == "0") CreateDesktopShortcut = false;
            }
            else if (key.Equals(BundleVar.CreateStartMenuShortcut, StringComparison.InvariantCultureIgnoreCase))
            {
                if (bool.TryParse(value, out var b)) CreateStartMenuShortcut = b;
                else if (value == "1") CreateStartMenuShortcut = true;
                else if (value == "0") CreateStartMenuShortcut = false;
            }
            else if (key.Equals(BundleVar.LaunchOnStartup, StringComparison.InvariantCultureIgnoreCase))
            {
                if (bool.TryParse(value, out var b)) LaunchOnStartup = b;
                else if (value == "1") LaunchOnStartup = true;
                else if (value == "0") LaunchOnStartup = false;
            }
        }
    }

    /// <summary>
    /// Sets the planned action for the bootstrapper and invokes the planning phase on the engine.
    /// </summary>
    /// <param name="action">The <see cref="LaunchAction"/> to plan.</param>
    private void Plan(LaunchAction action)
    {
        PlannedAction = action;
        _eyeDropperBA.Engine.Plan(PlannedAction);
    }

    /// <summary>
    /// Initiates layout planning by determining or prompting for the layout directory and then starting install or uninstall.
    /// </summary>
    private void PlanLayout()
    {
        if (string.IsNullOrEmpty(_eyeDropperBA.Command.LayoutDirectory))
        {
            LayoutDirectory = Directory.GetCurrentDirectory();

            if (Display == Display.Full)
            {
                _dispatcher.Invoke(() =>
                {
                    var browserDialog = new FolderBrowserDialog
                    {
                        RootFolder = Environment.SpecialFolder.MyComputer,
                        SelectedPath = LayoutDirectory
                    };
                    var result = browserDialog.ShowDialog();

                    if (DialogResult.OK == result)
                    {
                        LayoutDirectory = browserDialog.SelectedPath;

                        if (Action is LaunchAction.Uninstall)
                        {
                            Uninstall();
                        }
                        else
                        {
                            Install();
                        }
                    }
                    else
                    {
                        Close();
                    }
                });
            }
        }
        else
        {
            LayoutDirectory = _eyeDropperBA.Command.LayoutDirectory;

            if (Action is LaunchAction.Uninstall)
            {
                Uninstall();
            }
            else
            {
                Install();
            }
        }
    }

    /// <summary>
    /// Calculates overall progress percentage from cache and execution progress and raises the <see cref="OnProgress"/> event.
    /// </summary>
    private void ReportProgress()
    {
        var pct = GetProgress();
        OnProgress?.Invoke(this, new InstallationProgressEventArgs(pct));
    }

    /// <summary>
    /// Computes the combined progress across caching and execution phases.
    /// </summary>
    /// <returns>The overall progress percentage.</returns>
    private int GetProgress()
    {
        return (_cacheProgress + _executeProgress) / _progressPhases;
    }

    #endregion

    #region Events Subscriptions

    /// <summary>
    /// Handles the beginning of package detection by setting the detection state and resetting the planned action.
    /// </summary>
    /// <param name="sender">The source of the detection event.</param>
    /// <param name="e">Event arguments containing registration type.</param>
    private void DetectBegin(object? sender, DetectBeginEventArgs e)
    {
        InstallationState = InstallationState.Detecting;
        DetectState = RegistrationType.Full == e.RegistrationType ? DetectionState.Present : DetectionState.Absent;
        PlannedAction = LaunchAction.Unknown;
    }

    /// <summary>
    /// Called when detection completes; parses CLI, updates state, raises action request, and invokes automatic plans.
    /// </summary>
    /// <param name="sender">The source of the detection completion event.</param>
    /// <param name="e">Event arguments containing status and resume type.</param>
    private void DetectComplete(object? sender, DetectCompleteEventArgs e)
    {
        ParseCommandLine();
        InstallationState = InstallationState.Waiting;

        if (!HResult.Succeeded(e.Status))
        {
            InstallationState = InstallationState.Failed;
        }

        if (Action is not LaunchAction.Uninstall)
        {
            Downgrade = UpgradeDetectState == UpgradeDetectionState.Newer;
        }

        var eventArgs = new ActionRequestedEventArgs(_isRetry);
        _dispatcher.Invoke(() => OnActionRequested?.Invoke(this, eventArgs));

        if (eventArgs.Handled || InstallationState is InstallationState.Failed)
        {
            return;
        }

        if (Action is LaunchAction.Uninstall
            && _eyeDropperBA.Command.Resume is not ResumeType.Arp)
        {
            _eyeDropperBA.Engine.Log(LogLevel.Verbose, "Invoking automatic plan for uninstall");
            Uninstall();
        }
        else if (Action is LaunchAction.Layout)
        {
            PlanLayout();
        }
        else if (Display is not Display.Full)
        {
            _eyeDropperBA.Engine.Log(LogLevel.Verbose, "Invoking automatic plan for non-interactive mode.");

            if (Action is LaunchAction.Uninstall)
            {
                Uninstall();
            }
            else
            {
                Install();
            }
        }
    }

    /// <summary>
    /// Processes related bundle detection, sets existing version, and computes upgrade state.
    /// </summary>
    /// <param name="sender">The source of the related bundle event.</param>
    /// <param name="e">Event arguments containing related bundle details.</param>
    private void DetectRelatedBundle(object? sender, DetectRelatedBundleEventArgs e)
    {
        ExistingVersion = e.Version;

        if (e.RelationType is RelationType.Upgrade)
        {
            if (_eyeDropperBA.Engine.CompareVersions(Version, e.Version) >= 0)
            {
                if (UpgradeDetectState == UpgradeDetectionState.None)
                {
                    UpgradeDetectState = UpgradeDetectionState.Older;
                }
            }
            else
            {
                UpgradeDetectState = UpgradeDetectionState.Newer;
            }
        }

        if (!_eyeDropperBA.Manifest.Bundle.Packages.ContainsKey(e.ProductCode))
        {
            _eyeDropperBA.Manifest.Bundle.AddRelatedBundleAsPackage(e.ProductCode, e.RelationType, e.PerMachine, e.Version);
        }
    }

    /// <summary>
    /// Updates existing version if the core package is already present.
    /// </summary>
    /// <param name="sender">The source of the package complete event.</param>
    /// <param name="e">Event arguments containing package ID and state.</param>
    private void DetectPackageComplete(object? sender, DetectPackageCompleteEventArgs e)
    {
        if (e.PackageId.Equals(EyeDropperPackageId))
        {
            if (e.State is PackageState.Present)
            {
                ExistingVersion = Version;
            }
        }
    }

    /// <summary>
    /// Clears the package order when planning begins.
    /// </summary>
    /// <param name="sender">The source of the plan begin event.</param>
    /// <param name="e">Event arguments containing phase count.</param>
    private void PlanBegin(object? sender, PlanBeginEventArgs e)
    {
        lock (_syncRoot)
        {
            InstallationState = InstallationState.Planning;

            _packageOrder.Clear();
        }
    }

    /// <summary>
    /// Called when planning completes; initiates the apply phase or marks failure.
    /// </summary>
    /// <param name="sender">The source of the plan complete event.</param>
    /// <param name="args">Event arguments containing plan status.</param>
    private void PlanComplete(object? sender, PlanCompleteEventArgs args)
    {
        if (HResult.Succeeded(args.Status))
        {
            PreApplyState = InstallationState;
            InstallationState = InstallationState.Applying;

            _dispatcher.Invoke(() =>
            {
                var mainWindow = System.Windows.Application.Current?.MainWindow ?? new System.Windows.Window();
                var hwnd = new WindowInteropHelper(mainWindow).EnsureHandle();
                _eyeDropperBA.Engine.Apply(hwnd);
            });
        }
        else
        {
            InstallationState = InstallationState.Failed;

            var autoClose = _autoClose || Display is not Display.Full;
            var eventArgs = new ActionCompletedEventArgs(
                errorType: _errorType,
                autoClose: autoClose);

            _dispatcher.Invoke(() => OnActionCompleted?.Invoke(this, eventArgs));

            if (autoClose)
            {
                _eyeDropperBA.Engine.Log(LogLevel.Verbose, "Automatically closing the window.");

                Close();
                return;
            }
        }
    }

    /// <summary>
    /// Captures the total number of execution phases when apply begins.
    /// </summary>
    /// <param name="sender">The source of the apply begin event.</param>
    /// <param name="e">Event arguments containing phase count.</param>
    private void ApplyBegin(object? sender, ApplyBeginEventArgs e)
    {
        _progressPhases = e.PhaseCount;
    }

    /// <summary>
    /// Cancels apply progress update if the operation is canceled.
    /// </summary>
    /// <param name="sender">The source of the progress event.</param>
    /// <param name="e">Event arguments containing progress percentage.</param>
    private void Progress(object? sender, ProgressEventArgs e)
    {
        e.Cancel = Canceled;
    }

    /// <summary>
    /// Called when application completes; finalizes state, raises action completed, and optionally closes UI.
    /// </summary>
    /// <param name="sender">The source of the apply complete event.</param>
    /// <param name="e">Event arguments containing apply status.</param>
    private void ApplyComplete(object? sender, ApplyCompleteEventArgs e)
    {
        _result = e.Status;

        if (InstallationState != PreApplyState)
        {
            InstallationState = HResult.Succeeded(e.Status) ? InstallationState.Applied : InstallationState.Failed;
        }

        // Reset canceled state if not cancel not completed before success installation
        if (InstallationState is not InstallationState.Failed)
        {
            Canceled = false;
        }

        var isUpdateReplaceSucceeded = HResult.Succeeded(e.Status) && PlannedAction is LaunchAction.UpdateReplace;
        var autoClose = _autoClose || Display is not Display.Full || isUpdateReplaceSucceeded;

        var eventArgs = new ActionCompletedEventArgs(
            errorType: _errorType,
            autoClose: autoClose);

        _dispatcher.Invoke(() => OnActionCompleted?.Invoke(this, eventArgs));

        if (autoClose)
        {
            _eyeDropperBA.Engine.Log(LogLevel.Verbose, "Automatically closing the window.");

            Close();
            return;
        }
    }

    /// <summary>
    /// Handles engine errors, determining retry or cancel behavior based on state.
    /// </summary>
    /// <param name="sender">The source of the error event.</param>
    /// <param name="e">Event arguments containing error details.</param>
    private void Error(object? sender, ErrorEventArgs e)
    {
        lock (_syncRoot)
        {
            if (!Canceled)
            {
                if (InstallationState is InstallationState.Applying && e.ErrorCode == UserCancelledErrorCode)
                {
                    InstallationState = PreApplyState;
                }
                else if (Display is Display.Full
                    && e.ErrorType is ErrorType.HttpServerAuthentication or ErrorType.HttpProxyAuthentication)
                {
                    e.Result = Result.TryAgain;
                }
            }
            else
            {
                e.Result = Result.Cancel;
            }

            _errorType = e.ErrorType;
        }
    }

    /// <summary>
    /// Updates execution progress, sends embedded progress if needed, and raises overall progress.
    /// </summary>
    /// <param name="sender">The source of the execute progress event.</param>
    /// <param name="e">Event arguments containing progress percentages.</param>
    private void ExecuteProgress(object? sender, ExecuteProgressEventArgs e)
    {
        lock (_syncRoot)
        {
            _executeProgress = e.OverallPercentage;
            ReportProgress();

            var progress = GetProgress();

            if (Display is Display.Embedded)
                _eyeDropperBA.Engine.SendEmbeddedProgress(e.ProgressPercentage, progress);

            e.Cancel = Canceled;
        }
    }

    /// <summary>
    /// Records each planned package with an execution order index.
    /// </summary>
    /// <param name="sender">The source of the planned package event.</param>
    /// <param name="e">Event arguments containing package ID and execute state.</param>
    private void PlannedPackage(object? sender, PlannedPackageEventArgs e)
    {
        if (e.Execute is not ActionState.None)
        {
            lock (_syncRoot)
            {
                _packageOrder.Add(e.PackageId, _packageOrder.Count);
            }
        }
    }

    /// <summary>
    /// Updates cache acquisition progress and raises overall progress.
    /// </summary>
    /// <param name="sender">The source of the cache acquire event.</param>
    /// <param name="e">Event arguments containing overall percentage.</param>
    private void CacheAcquireProgress(object? sender, CacheAcquireProgressEventArgs e)
    {
        lock (_syncRoot)
        {
            _cacheProgress = e.OverallPercentage;
            ReportProgress();
            e.Cancel = Canceled;
        }
    }

    /// <summary>
    /// Updates cache verification progress and raises overall progress.
    /// <param name="sender">The source of the verify event.</param>
    /// <param name="e">Event arguments containing overall percentage.</param>
    private void CacheContainerOrPayloadVerifyProgress(object? sender, CacheContainerOrPayloadVerifyProgressEventArgs e)
    {
        lock (_syncRoot)
        {
            _cacheProgress = e.OverallPercentage;
            ReportProgress();
            e.Cancel = Canceled;
        }
    }

    /// <summary>
    /// Updates payload extract progress and raises overall progress.
    /// </summary>
    /// <param name="sender">The source of the payload extract event.</param>
    /// <param name="e">Event arguments containing overall percentage.</param>
    private void CachePayloadExtractProgress(object? sender, CachePayloadExtractProgressEventArgs e)
    {
        lock (_syncRoot)
        {
            _cacheProgress = e.OverallPercentage;
            ReportProgress();
            e.Cancel = Canceled;
        }
    }

    /// <summary>
    /// Updates cache verification progress and raises overall progress.
    /// </summary>
    /// <param name="sender">The source of the cache verify event.</param>
    /// <param name="e">Event arguments containing overall percentage.</param>
    private void CacheVerifyProgress(object? sender, CacheVerifyProgressEventArgs e)
    {
        lock (_syncRoot)
        {
            _cacheProgress = e.OverallPercentage;
            ReportProgress();
            e.Cancel = Canceled;
        }
    }

    /// <summary>
    /// Sets cache progress to 100% when caching completes and raises overall progress.
    /// </summary>
    /// <param name="sender">The source of the cache complete event.</param>
    /// <param name="e">Event arguments for cache completion.</param>
    private void CacheComplete(object? sender, CacheCompleteEventArgs e)
    {
        lock (_syncRoot)
        {
            _cacheProgress = 100;
            ReportProgress();
        }
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Releases resources used by this instance.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release managed resources; otherwise <c>false</c>.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                UnwireEvents();
            }

            _disposedValue = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}