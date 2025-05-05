using System.Windows.Threading;
using EyeDropper.Bootstrapper.Core.Contracts;
using WixToolset.BootstrapperApplicationApi;

namespace EyeDropper.Bootstrapper.Host.WixToolset;

/// <summary>
/// Main bootstrapper application class for the installer.
/// Manages the installation workflow and handles events from the WiX engine.
/// </summary>
public class WpfBootstrapperApplication : BootstrapperApplication, IWpfBootstrapperApplication
{
    #region Fields/Consts

    private IBootstrapperCommand _command = null!;
    private BootstrapperApplicationData _manifest = null!;
    private WpfBootstrapperApplicationManager _bootstrapperApplicationManager = null!;

    private static readonly WpfBootstrapperApplication _current = new();

    #endregion

    #region Properties

    /// <summary>
    /// Gets the singleton instance of the EyeDropperBA.
    /// </summary>
    public static WpfBootstrapperApplication Current => _current;

    /// <summary>
    /// Gets the WiX engine interface for logging and interacting with the installation.
    /// </summary>
    public IEngine Engine => engine;

    /// <summary>
    /// Gets the bootstrapper command data provided at startup.
    /// </summary>
    public IBootstrapperCommand Command => _command;

    /// <summary>
    /// Gets the manifest data for the bootstrapper application.
    /// </summary>
    public IBootstrapperApplicationData Manifest => _manifest;

    /// <summary>
    /// Gets the manager for the bootstrapper application.
    /// </summary>
    public IWpfBootstrapperApplicationManager BootstrapperApplicationManager => _bootstrapperApplicationManager;

    #endregion

    /// <summary>
    /// Private constructor to enforce singleton pattern.
    /// </summary>
    private WpfBootstrapperApplication()
    {
    }

    #region Methods Overrides

    /// <summary>
    /// Entry point for the bootstrapper application.
    /// Begins the installation process and quits when complete.
    /// </summary>
    protected override void Run()
    {
        Engine.Log(LogLevel.Verbose, "Running the WiX BA.");
        var dispatcher = Dispatcher.CurrentDispatcher;

        _bootstrapperApplicationManager = new WpfBootstrapperApplicationManager(this, dispatcher);

        var exitCode = _bootstrapperApplicationManager.Run();
        Engine.Log(LogLevel.Verbose, $"BA Manager returned exit code: {exitCode}.");

        _bootstrapperApplicationManager.Dispose();

        if ((exitCode & 0xFFFF0000) == unchecked((int)0x80070000))
        {
            // Convert HRESULT to Win32 error code.
            exitCode &= 0xFFFF;
            Engine.Log(LogLevel.Verbose, $"Converted HRESULT to Win32 error code: {exitCode}.");
        }

        Engine.Log(LogLevel.Standard, $"Quitting bootstrapper with exit code: {exitCode}.");
        Engine.Quit(exitCode);
    }

    /// <summary>
    /// Initializes the bootstrapper application with data from the Create event.
    /// </summary>
    /// <param name="args">Event arguments containing command and other data.</param>
    protected override void OnCreate(CreateEventArgs args)
    {
        base.OnCreate(args);

        Engine.Log(LogLevel.Verbose, "OnCreate event fired.");
        _command = args.Command;
        Engine.Log(LogLevel.Debug, $"Bootstrapper command initialized: {_command.Action}");
        _manifest = new BootstrapperApplicationData();
        Engine.Log(LogLevel.Debug, "Bootstrapper manifest data initialized.");
    }

    /// <summary>
    /// Handles the beginning of planning for each package.
    /// Skips installation of .NET Framework or Desktop .NET Core if already present.
    /// </summary>
    /// <param name="args">Event arguments specifying package details and planning state.</param>
    protected override void OnPlanPackageBegin(PlanPackageBeginEventArgs args)
    {
        base.OnPlanPackageBegin(args);

        Engine.Log(LogLevel.Verbose, $"Planning package: {args.PackageId}, initial state: {args.State}.");

        // If our bootstrapper can run, skip .NET installations.
        if (args.PackageId.StartsWith("NetFx4", StringComparison.OrdinalIgnoreCase)
            || args.PackageId.StartsWith("DesktopNetCoreRuntime", StringComparison.OrdinalIgnoreCase))
        {
            args.State = RequestState.None;
            Engine.Log(LogLevel.Standard, $"Skipping package: {args.PackageId} as .NET is already present.");
        }
        else
        {
            Engine.Log(LogLevel.Standard, $"Scheduling package for install: {args.PackageId}.");
        }
    }

    #endregion
}
