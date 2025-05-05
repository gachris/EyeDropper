namespace EyeDropper.UI.DialogWindows;

/// <summary>
/// Represents a custom button definition provided by a plugin for inclusion in a dialog.
/// </summary>
public class PluginButton
{
    #region Properties

    /// <summary>
    /// Gets or sets the content to display on the button (e.g., text or UI element).
    /// </summary>
    public object? Content { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this button should be the default action
    /// (activated when the user presses Enter).
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets the type of action this button represents (e.g., OK, Cancel).
    /// </summary>
    public PluginButtonType ButtonType { get; set; }

    /// <summary>
    /// Gets or sets the position of the button in the dialog footer (left or right).
    /// </summary>
    public PluginButtonPosition ButtonPosition { get; set; }

    /// <summary>
    /// Gets or sets the order in which this button appears relative to other plugin buttons.
    /// </summary>
    public int ButtonOrder { get; set; }

    #endregion
}
