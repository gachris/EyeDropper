namespace EyeDropper.Bootstrapper.Core;

/// <summary>
/// Represents the state of upgrade detection relative to other bundles.
/// </summary>
public enum UpgradeDetectionState
{
    /// <summary>
    /// No related bundles are installed.
    /// </summary>
    None,

    /// <summary>
    /// All installed related bundles are older than or the same version as this bundle.
    /// </summary>
    Older,

    /// <summary>
    /// At least one installed related bundle is newer than this bundle.
    /// </summary>
    Newer,
}