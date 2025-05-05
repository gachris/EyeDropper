namespace EyeDropper.Bootstrapper.Host.WixToolset;

/// <summary>
/// Defines the bundle variable names used by the WiX Toolset
/// for the EyeDropper bootstrapper host.
/// </summary>
internal static class BundleVar
{
    /// <summary>
    /// Gets the variable name for the installation directory.
    /// </summary>
    public const string InstallDirectory = "InstallFolder";

    /// <summary>
    /// Gets the variable name for the layout directory
    /// where bundle payloads are cached.
    /// </summary>
    public const string LayoutDirectory = "WixBundleLayoutDirectory";

    /// <summary>
    /// Gets the variable name for the bundle version.
    /// </summary>
    public const string Version = "WixBundleVersion";

    /// <summary>
    /// Gets the variable name for the bundle display name.
    /// </summary>
    public const string Name = "WixBundleName";

    /// <summary>
    /// Gets the variable name controlling whether a desktop shortcut
    /// should be created during installation.
    /// </summary>
    public const string CreateDesktopShortcut = "CreateDesktopShortcut";

    /// <summary>
    /// Gets the variable name controlling whether a start menu
    /// should be created during installation.
    /// </summary>
    public const string CreateStartMenuShortcut = "CreateStartMenuShortcut";

    /// <summary>
    /// Gets the variable name controlling whether the application
    /// should launch on startup.
    /// </summary>
    public const string LaunchOnStartup = "LaunchOnStartup";

    /// <summary>
    /// Gets the variable name for the bundle log file path.
    /// </summary>
    public const string Log = "WixBundleLog";

    /// <summary>
    /// Gets the variable name for the license agreement file.
    /// </summary>
    public const string License = "License";
}
