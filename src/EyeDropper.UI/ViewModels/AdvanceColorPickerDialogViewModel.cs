using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevToolbox.Wpf.Controls;
using EyeDropper.ApplicationFlow;
using EyeDropper.Core.RuntimeOptions;
using EyeDropper.Core.RuntimeOptions.Events;
using EyeDropper.UI.DialogWindows;
using EyeDropper.UI.DialogWindows.ViewModels;

namespace EyeDropper.UI.ViewModels;

/// <summary>
/// ViewModel for the advanced color picker dialog, handling template updates and capture events.
/// </summary>
public partial class AdvanceColorPickerDialogViewModel : DialogViewModel
{
    #region Fields/Consts

    private readonly IRuntimeOptionsState _runtimeInfo;

    private Brush? _capturedColor;

    /// <summary>
    /// Gets or sets the currently selected color in the dialog.
    /// </summary>
    [ObservableProperty]
    private Color _selectedColor = Colors.Black;

    /// <summary>
    /// Gets or sets the initial color value when the dialog is first opened.
    /// </summary>
    [ObservableProperty]
    private Color _initialColor = Colors.Black;

    /// <summary>
    /// Gets or sets a value indicating whether the formatted color string should be copied to the clipboard.
    /// </summary>
    [ObservableProperty]
    private bool _copyToClipboard;

    /// <summary>
    /// Gets or sets the format in which the color is represented (e.g., Hex, RGB, HSL).
    /// </summary>
    [ObservableProperty]
    private ColorFormat _colorFormat;

    /// <summary>
    /// Gets or sets the template string used to render the color according to the selected format.
    /// </summary>
    [ObservableProperty]
    private string _template = null!;

    /// <summary>
    /// Gets or sets the number of decimal places to use when formatting numeric color components.
    /// </summary>
    [ObservableProperty]
    private int _precision = 1;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="AdvanceColorPickerDialogViewModel"/> class.
    /// Subscribes to runtime option change events to keep UI in sync.
    /// </summary>
    /// <param name="runtimeOptionsState">Provides access to current runtime options and templates.</param>
    /// <param name="applicationEvents">Publishes events when runtime options are updated.</param>
    public AdvanceColorPickerDialogViewModel(
        IRuntimeOptionsState runtimeOptionsState,
        IApplicationEvents applicationEvents)
    {
        _runtimeInfo = runtimeOptionsState;

        applicationEvents.Register<ColorFormatUpdatedEvent>((e) =>
        {
            ColorFormat = (ColorFormat)_runtimeInfo.ColorFormat;
            UpdateSelectedTemplate();
        });

        applicationEvents.Register<HTMLTemplateUpdatedEvent>((e) => UpdateSelectedTemplate());
        applicationEvents.Register<HexTemplateUpdatedEvent>((e) => UpdateSelectedTemplate());
        applicationEvents.Register<DelphiHexTemplateUpdatedEvent>((e) => UpdateSelectedTemplate());
        applicationEvents.Register<VBHexTemplateUpdatedEvent>((e) => UpdateSelectedTemplate());
        applicationEvents.Register<RGBTemplateUpdatedEvent>((e) => UpdateSelectedTemplate());
        applicationEvents.Register<RGBFloatTemplateUpdatedEvent>((e) => UpdateSelectedTemplate());
        applicationEvents.Register<HSVTemplateUpdatedEvent>((e) => UpdateSelectedTemplate());
        applicationEvents.Register<HSLTemplateUpdatedEvent>((e) => UpdateSelectedTemplate());
        applicationEvents.Register<LongTemplateUpdatedEvent>((e) => UpdateSelectedTemplate());
        applicationEvents.Register<PrecisionUpdatedEvent>((e) => Precision = _runtimeInfo.Precision);
        applicationEvents.Register<CopyToClipboardUpdatedEvent>((e) => CopyToClipboard = _runtimeInfo.CopyToClipboard);

        Precision = _runtimeInfo.Precision;
        CopyToClipboard = _runtimeInfo.CopyToClipboard;
        ColorFormat = (ColorFormat)_runtimeInfo.ColorFormat;
        UpdateSelectedTemplate();
    }

    #region Methods

    /// <summary>
    /// Updates the <see cref="Template"/> property based on the current <see cref="ColorFormat"/>.
    /// </summary>
    private void UpdateSelectedTemplate()
    {
        Template = ColorFormat switch
        {
            ColorFormat.HTML => _runtimeInfo.HTMLTemplate,
            ColorFormat.Hex => _runtimeInfo.HexTemplate,
            ColorFormat.DelphiHex => _runtimeInfo.DelphiHexTemplate,
            ColorFormat.VBHex => _runtimeInfo.VBHexTemplate,
            ColorFormat.RGB => _runtimeInfo.RGBTemplate,
            ColorFormat.RGBFloat => _runtimeInfo.RGBFloatTemplate,
            ColorFormat.HSV => _runtimeInfo.HSVTemplate,
            ColorFormat.HSL => _runtimeInfo.HSLTemplate,
            ColorFormat.Long => _runtimeInfo.LongTemplate,
            _ => throw new ArgumentOutOfRangeException(nameof(ColorFormat), ColorFormat, "Unsupported color format")
        };
    }

    #endregion

    #region Relay Command

    /// <summary>
    /// OK command: sets the dialog result to OK and updates InitialColor to the currently selected color.
    /// </summary>
    [RelayCommand]
    private void OK()
    {
        HostWindow.ModalResult = ModalResult.OK;
        InitialColor = SelectedColor;
    }

    /// <summary>
    /// CaptureChanged command: updates SelectedColor when the capture finishes.
    /// </summary>
    /// <param name="e">The capture event arguments.</param>
    [RelayCommand]
    private void CaptureChanged(CaptureEventArgs e)
    {
        if (e.CaptureState == CaptureState.Finished)
        {
            SelectedColor = (_capturedColor as SolidColorBrush)?.Color ?? Colors.Black;
        }
    }

    /// <summary>
    /// ColorChanged command: stores the current brush provided by the capture.
    /// </summary>
    /// <param name="brush">The captured brush.</param>
    [RelayCommand]
    private void ColorChanged(Brush brush)
    {
        _capturedColor = brush;
    }

    #endregion
}
