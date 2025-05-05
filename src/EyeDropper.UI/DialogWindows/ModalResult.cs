namespace EyeDropper.UI.DialogWindows;

/// <summary>
/// Specifies the result returned by a modal dialog window.
/// </summary>
public enum ModalResult
{
    /// <summary>
    /// The user accepted or confirmed the dialog.
    /// </summary>
    OK,

    /// <summary>
    /// The user canceled or dismissed the dialog.
    /// </summary>
    Cancel,

    /// <summary>
    /// The user answered “Yes” to a confirmation dialog.
    /// </summary>
    Yes,

    /// <summary>
    /// The user answered “No” to a confirmation dialog.
    /// </summary>
    No
}
