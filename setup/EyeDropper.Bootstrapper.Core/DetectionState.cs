namespace EyeDropper.Bootstrapper.Core;

/// <summary>
/// Represents the detection state of a product on the target system.
/// </summary>
public enum DetectionState
{
    /// <summary>
    /// The product is not detected on the system.
    /// </summary>
    Absent,

    /// <summary>
    /// The product is detected and present on the system.
    /// </summary>
    Present,
}
