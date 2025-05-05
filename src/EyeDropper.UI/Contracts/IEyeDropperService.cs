namespace EyeDropper.UI.Contracts;

/// <summary>
/// Defines the contract for a service that handles color capture operations.
/// </summary>
public interface IEyeDropperService
{
    /// <summary>
    /// Initiates the color capture process, allowing the user to select a point on the screen.
    /// </summary>
    void StartCapture();
}
