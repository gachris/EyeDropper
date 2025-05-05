using EyeDropper.UI.DialogWindows.ViewModels;

namespace EyeDropper.UI.ViewModels;

/// <summary>
/// ViewModel for displaying error information in a dialog.
/// </summary>
public partial class ErrorDialogViewModel : DialogViewModel
{
    #region Properties

    /// <summary>
    /// Gets the error message to display in the dialog.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the optional stack trace of the error.
    /// </summary>
    public string? StackTrace { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorDialogViewModel"/> class with
    /// the specified error message and stack trace.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    /// <param name="stackTrace">The optional stack trace associated with the error.</param>
    public ErrorDialogViewModel(string message, string? stackTrace)
    {
        Message = message;
        StackTrace = stackTrace;
    }

    #endregion
}