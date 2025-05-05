namespace EyeDropper.Bootstrapper.Core;

/// <summary>
/// Provides data for the <c>ActionRequested</c> event, including the current installation state,
/// the action to be launched, display options, and any downgrade detection.
/// </summary>
public class ActionRequestedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets a value indicating whether the action is retrying after user canceled.
    /// </summary>
    public bool IsRetry { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the event has been handled.
    /// </summary>
    public bool Handled { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionRequestedEventArgs"/> class.
    /// </summary>
    public ActionRequestedEventArgs(bool isRetry)
    {
        IsRetry = isRetry;
    }
}
