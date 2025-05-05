namespace EyeDropper.Bootstrapper.Core;

/// <summary>
/// Provides data for the <c>Progress</c> event during an installation,
/// indicating the percentage of completion.
/// </summary>
public class InstallationProgressEventArgs : EventArgs
{
    /// <summary>
    /// Gets the current installation progress as a percentage (0–100).
    /// </summary>
    public int Progress { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallationProgressEventArgs"/> class.
    /// </summary>
    /// <param name="progress">The installation progress percentage.</param>
    public InstallationProgressEventArgs(int progress)
    {
        Progress = progress;
    }
}