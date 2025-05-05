using WixToolset.BootstrapperApplicationApi;

namespace EyeDropper.Bootstrapper.Core;

/// <summary>
/// Provides data for the <c>ActionCompleted</c> event.
/// </summary>
public class ActionCompletedEventArgs : EventArgs
{
    /// <summary>
    /// Gets a value indicating whether the bootstrapper should automatically close
    /// once the action is complete.
    /// </summary>
    public bool AutoClose { get; }

    /// <summary>
    /// Gets the type of error that occurred, if any.
    /// </summary>
    public ErrorType? ErrorType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionCompletedEventArgs"/> class.
    /// </summary>
    /// <param name="autoClose">Whether to close the UI automatically after completion.</param>
    /// <param name="errorType">The type of error encountered, if any.</param>
    public ActionCompletedEventArgs(
        bool autoClose,
        ErrorType? errorType)
    {
        AutoClose = autoClose;
        ErrorType = errorType;
    }
}
