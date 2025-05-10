using CommunityToolkit.Mvvm.ComponentModel;
using DevToolbox.Wpf.Controls;
using DevToolbox.Core.ApplicationFlow;
using EyeDropper.Core.RuntimeOptions;
using EyeDropper.Core.RuntimeOptions.Events;

namespace EyeDropper.UI.ViewModels;

/// <summary>
/// ViewModel for the EyeDropper host, handling runtime option changes and formatting logic.
/// </summary>
public partial class EyeDropperViewModel : ObservableObject
{
    #region Fields/Consts

    private readonly IRuntimeOptionsState _runtimeInfo;

    /// <summary>
    /// Gets or sets a value indicating whether the formatted color string
    /// should be automatically copied to the clipboard.
    /// </summary>
    [ObservableProperty]
    private bool _copyToClipboard;

    /// <summary>
    /// Gets or sets the selected <see cref="ColorFormat"/> used to format the captured color.
    /// </summary>
    [ObservableProperty]
    private ColorFormat _colorFormat;

    /// <summary>
    /// Gets or sets the template string that determines how the color is rendered
    /// according to the selected format.
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
    /// Initializes a new instance of the <see cref="EyeDropperViewModel"/> class.
    /// Subscribes to runtime option events to update UI properties automatically.
    /// </summary>
    /// <param name="runtimeOptionsState">Provides access to current runtime options and templates.</param>
    /// <param name="applicationEvents">Publishes events when runtime options are updated.</param>
    public EyeDropperViewModel(
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
    /// Updates the <see cref="Template"/> based on the current <see cref="ColorFormat"/>.
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
}
