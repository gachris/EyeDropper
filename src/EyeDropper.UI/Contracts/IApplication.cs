namespace EyeDropper.UI.Contracts;

/// <summary>
/// Represents the core application functionality.
/// </summary>
public interface IApplication
{
    /// <summary>
    /// Shuts down the application, closing all windows and exiting the process.
    /// </summary>
    void Shutdown();
}
