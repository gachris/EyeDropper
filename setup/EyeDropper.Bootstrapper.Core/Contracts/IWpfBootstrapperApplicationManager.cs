using WixToolset.BootstrapperApplicationApi;

namespace EyeDropper.Bootstrapper.Core.Contracts;

/// <summary>
/// Provides control over and information about the bootstrapper application’s installation process.
/// </summary>
public interface IWpfBootstrapperApplicationManager
{
    /// <summary>
    /// Occurs when the bootstrapper requests that an action (install, uninstall, repair, etc.) be performed.
    /// </summary>
    event EventHandler<ActionRequestedEventArgs>? OnActionRequested;

    /// <summary>
    /// Occurs when a previously requested action completes.
    /// </summary>
    event EventHandler<ActionCompletedEventArgs>? OnActionCompleted;

    /// <summary>
    /// Occurs to report progress during installation or uninstallation.
    /// </summary>
    event EventHandler<InstallationProgressEventArgs>? OnProgress;

    /// <summary>
    /// Occurs when the current operation is canceled.
    /// </summary>
    event EventHandler? OnCanceled;

    /// <summary>
    /// Gets the action that the bootstrapper is to perform (e.g., install, uninstall, repair).
    /// </summary>
    LaunchAction Action { get; }

    /// <summary>
    /// Gets the action that the engine has planned to perform.
    /// </summary>
    LaunchAction PlannedAction { get; }

    /// <summary>
    /// Gets the display mode of the bootstrapper (e.g., full UI, passive, or silent).
    /// </summary>
    Display Display { get; }

    /// <summary>
    /// Gets the current installation state.
    /// </summary>
    InstallationState InstallationState { get; }

    /// <summary>
    /// Gets the installation state immediately before the apply phase begins.
    /// </summary>
    InstallationState PreApplyState { get; }

    /// <summary>
    /// Gets the current detection state (whether the product is present).
    /// </summary>
    DetectionState DetectState { get; }

    /// <summary>
    /// Gets the state of upgrade detection.
    /// </summary>
    UpgradeDetectionState UpgradeDetectState { get; }

    /// <summary>
    /// Gets the name of the bundle being installed.
    /// </summary>
    string BundleName { get; }

    /// <summary>
    /// Gets the URL of the license agreement to display.
    /// </summary>
    string LicenseUrl { get; }

    /// <summary>
    /// Gets the version of the bundle being installed.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Gets the existing installed version, if any.
    /// </summary>
    string? ExistingVersion { get; }

    /// <summary>
    /// Gets the full path to the bootstrapper’s log file.
    /// </summary>
    string LogFilePath { get; }

    /// <summary>
    /// Gets the directory containing the layout (extracted payload) for installation.
    /// </summary>
    string LayoutDirectory { get; }

    /// <summary>
    /// Gets or sets the target directory where the product will be installed.
    /// </summary>
    string InstallDirectory { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a desktop shortcut should be created.
    /// </summary>
    bool CreateDesktopShortcut { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a start menu shortcut should be created.
    /// </summary>
    bool CreateStartMenuShortcut { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the application should be launched on system startup.
    /// </summary>
    bool LaunchOnStartup { get; set; }

    /// <summary>
    /// Gets a value indicating whether a downgrade (installation of an older version over a newer one) is allowed.
    /// </summary>
    bool Downgrade { get; }

    /// <summary>
    /// Gets a value indicating whether the current operation has been canceled.
    /// </summary>
    bool Canceled { get; }

    /// <summary>
    /// Starts the installation operation.
    /// </summary>
    void Install();

    /// <summary>
    /// Starts the uninstallation operation.
    /// </summary>
    void Uninstall();

    /// <summary>
    /// Cancels the current operation.
    /// </summary>
    /// <param name="autoClose">
    /// If set to <c>true</c>, the bootstrapper will automatically close its window after canceling.
    /// </param>
    void Cancel(bool autoClose = false);

    /// <summary>
    /// Closes the bootstrapper application.
    /// </summary>
    void Close();

    /// <summary>
    /// Resets the state after user canceled the installation.
    /// </summary>
    void ResetState();

    /// <summary>
    /// Closes the splash screen if one is displayed.
    /// </summary>
    void CloseSplashScreen();
}