using System.Windows;

namespace EyeDropper.UI.DialogWindows;

/// <summary>
/// Encapsulates options for configuring the appearance and behavior of a dialog window.
/// </summary>
public class DialogOptions
{
    #region Properties

    /// <summary>
    /// Gets or sets the minimum width of the dialog window.
    /// </summary>
    public double MinWidth { get; set; }

    /// <summary>
    /// Gets or sets the minimum height of the dialog window.
    /// </summary>
    public double MinHeight { get; set; }

    /// <summary>
    /// Gets or sets the initial width of the dialog window.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the initial height of the dialog window.
    /// </summary>
    public double Height { get; set; }

    /// <summary>
    /// Gets or sets the maximum width of the dialog window.
    /// </summary>
    public double MaxWidth { get; set; } = double.PositiveInfinity;

    /// <summary>
    /// Gets or sets the maximum height of the dialog window.
    /// </summary>
    public double MaxHeight { get; set; } = double.PositiveInfinity;

    /// <summary>
    /// Gets or sets how the dialog window sizes itself with respect to its content.
    /// </summary>
    public SizeToContent SizeToContent { get; set; }

    /// <summary>
    /// Gets or sets the title text displayed in the dialog window's title bar.
    /// </summary>
    public string? WindowTitle { get; set; }

    /// <summary>
    /// Gets or sets the title text shown in the waiting animation, if any.
    /// </summary>
    public string? AnimationTitle { get; set; }

    /// <summary>
    /// Gets or sets the message text shown in the waiting animation, if any.
    /// </summary>
    public string? AnimationMessage { get; set; }

    /// <summary>
    /// Gets or sets any plugin-provided buttons to display in the dialog footer.
    /// </summary>
    public PluginButton[]? PluginButtons { get; set; }

    /// <summary>
    /// Gets or sets the resize mode of the dialog window (e.g., CanResize, NoResize).
    /// </summary>
    public ResizeMode ResizeMode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog window's title bar is visible.
    /// </summary>
    public bool IsTitleBarVisible { get; set; } = true;

    #endregion
}
