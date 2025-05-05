namespace EyeDropper.UI.DialogWindows;

/// <summary>
/// Specifies the semantic type of a plugin-provided button for dialog windows.
/// </summary>
public enum PluginButtonType
{
    /// <summary>
    /// Indicates an affirmative action, typically confirming or accepting the dialog.
    /// </summary>
    OK,

    /// <summary>
    /// Indicates a request to close the dialog without committing changes.
    /// </summary>
    Close,

    /// <summary>
    /// Indicates a cancellation action, typically dismissing the dialog without saving.
    /// </summary>
    Cancel
}
