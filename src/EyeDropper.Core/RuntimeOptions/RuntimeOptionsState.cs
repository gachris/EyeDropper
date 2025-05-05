using EyeDropper.ApplicationFlow;
using EyeDropper.Core.RuntimeOptions.Events;

namespace EyeDropper.Core.RuntimeOptions;

/// <summary>
/// Stores and manages the runtime options state for the EyeDropper application.
/// Raises domain events when individual settings are updated.
/// </summary>
public class RuntimeOptionsState : AggregateRoot, IRuntimeOptionsState
{
    #region Properties

    /// <summary>
    /// Gets a value indicating whether the captured color should be copied to the clipboard automatically.
    /// </summary>
    public bool CopyToClipboard { get; private set; }

    /// <summary>
    /// Gets the key code for the primary modifier used when activating the color picker.
    /// </summary>
    public int PrimaryModifierKey { get; private set; }

    /// <summary>
    /// Gets the key code for the secondary modifier used when activating the color picker.
    /// </summary>
    public int SecondaryModifierKey { get; private set; }

    /// <summary>
    /// Gets the key code for the tertiary modifier used when activating the color picker.
    /// </summary>
    public int TertiaryModifierKey { get; private set; }

    /// <summary>
    /// Gets the key code for the hotkey that triggers color capture.
    /// </summary>
    public int HotKey { get; private set; }

    /// <summary>
    /// Gets the currently selected color format enum value.
    /// </summary>
    public int ColorFormat { get; private set; }

    /// <summary>
    /// Gets the number of decimal places used for formatting floating-point color components.
    /// </summary>
    public int Precision { get; private set; }

    /// <summary>
    /// Gets the template string used for HTML color output.
    /// </summary>
    public string HTMLTemplate { get; private set; } = null!;

    /// <summary>
    /// Gets the template string used for hexadecimal color output.
    /// </summary>
    public string HexTemplate { get; private set; } = null!;

    /// <summary>
    /// Gets the template string used for Delphi-style hexadecimal color output.
    /// </summary>
    public string DelphiHexTemplate { get; private set; } = null!;

    /// <summary>
    /// Gets the template string used for VB-style hexadecimal color output.
    /// </summary>
    public string VBHexTemplate { get; private set; } = null!;

    /// <summary>
    /// Gets the template string used for RGB color output.
    /// </summary>
    public string RGBTemplate { get; private set; } = null!;

    /// <summary>
    /// Gets the template string used for floating-point RGB color output.
    /// </summary>
    public string RGBFloatTemplate { get; private set; } = null!;

    /// <summary>
    /// Gets the template string used for HSV color output.
    /// </summary>
    public string HSVTemplate { get; private set; } = null!;

    /// <summary>
    /// Gets the template string used for HSL color output.
    /// </summary>
    public string HSLTemplate { get; private set; } = null!;

    /// <summary>
    /// Gets the template string used for long integer color output.
    /// </summary>
    public string LongTemplate { get; private set; } = null!;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="RuntimeOptionsState"/> class.
    /// </summary>
    public RuntimeOptionsState()
    {
    }

    #region Methods

    /// <summary>
    /// Updates the copy-to-clipboard setting and raises a <see cref="CopyToClipboardUpdatedEvent"/>.
    /// </summary>
    /// <param name="copyToClipboard">True to enable automatic copying; otherwise, false.</param>
    public void UpdateCopyToClipboard(bool copyToClipboard)
    {
        CopyToClipboard = copyToClipboard;
        AddEvent(new CopyToClipboardUpdatedEvent());
    }

    /// <summary>
    /// Updates the primary modifier key code and raises a <see cref="PrimaryModifierKeyUpdatedEvent"/>.
    /// </summary>
    /// <param name="primaryModifierKey">Key code for the primary modifier.</param>
    public void UpdatePrimaryModifierKey(int primaryModifierKey)
    {
        PrimaryModifierKey = primaryModifierKey;
        AddEvent(new PrimaryModifierKeyUpdatedEvent());
    }

    /// <summary>
    /// Updates the secondary modifier key code and raises a <see cref="SecondaryModifierKeyUpdatedEvent"/>.
    /// </summary>
    /// <param name="secondaryModifierKey">Key code for the secondary modifier.</param>
    public void UpdateSecondaryModifierKey(int secondaryModifierKey)
    {
        SecondaryModifierKey = secondaryModifierKey;
        AddEvent(new SecondaryModifierKeyUpdatedEvent());
    }

    /// <summary>
    /// Updates the tertiary modifier key code and raises a <see cref="TertiaryModifierKeyUpdatedEvent"/>.
    /// </summary>
    /// <param name="tertiaryModifierKey">Key code for the tertiary modifier.</param>
    public void UpdateTertiaryModifierKey(int tertiaryModifierKey)
    {
        TertiaryModifierKey = tertiaryModifierKey;
        AddEvent(new TertiaryModifierKeyUpdatedEvent());
    }

    /// <summary>
    /// Updates the hotkey code and raises a <see cref="HotKeyUpdatedEvent"/>.
    /// </summary>
    /// <param name="hotKey">Key code for the hotkey trigger.</param>
    public void UpdateHotKey(int hotKey)
    {
        HotKey = hotKey;
        AddEvent(new HotKeyUpdatedEvent());
    }

    /// <summary>
    /// Updates the color format and raises a <see cref="ColorFormatUpdatedEvent"/>.
    /// </summary>
    /// <param name="colorFormat">Enum value representing the selected format.</param>
    public void UpdateColorFormat(int colorFormat)
    {
        ColorFormat = colorFormat;
        AddEvent(new ColorFormatUpdatedEvent());
    }

    /// <summary>
    /// Updates the precision for floating-point color components and raises a <see cref="PrecisionUpdatedEvent"/>.
    /// </summary>
    /// <param name="precision">Number of decimal places.</param>
    public void UpdatePrecision(int precision)
    {
        Precision = precision;
        AddEvent(new PrecisionUpdatedEvent());
    }

    /// <summary>
    /// Updates the HTML color output template and raises a <see cref="HTMLTemplateUpdatedEvent"/>.
    /// </summary>
    /// <param name="htmlTemplate">Template string for HTML format.</param>
    public void UpdateHTMLTemplate(string htmlTemplate)
    {
        HTMLTemplate = htmlTemplate;
        AddEvent(new HTMLTemplateUpdatedEvent());
    }

    /// <summary>
    /// Updates the hexadecimal color output template and raises a <see cref="HexTemplateUpdatedEvent"/>.
    /// </summary>
    /// <param name="hexTemplate">Template string for hex format.</param>
    public void UpdateHexTemplate(string hexTemplate)
    {
        HexTemplate = hexTemplate;
        AddEvent(new HexTemplateUpdatedEvent());
    }

    /// <summary>
    /// Updates the Delphi hex output template and raises a <see cref="DelphiHexTemplateUpdatedEvent"/>.
    /// </summary>
    /// <param name="delphiHexTemplate">Template string for Delphi hex format.</param>
    public void UpdateDelphiHexTemplate(string delphiHexTemplate)
    {
        DelphiHexTemplate = delphiHexTemplate;
        AddEvent(new DelphiHexTemplateUpdatedEvent());
    }

    /// <summary>
    /// Updates the VB hex output template and raises a <see cref="VBHexTemplateUpdatedEvent"/>.
    /// </summary>
    /// <param name="vbHexTemplate">Template string for VB hex format.</param>
    public void UpdateVBHexTemplate(string vbHexTemplate)
    {
        VBHexTemplate = vbHexTemplate;
        AddEvent(new VBHexTemplateUpdatedEvent());
    }

    /// <summary>
    /// Updates the RGB output template and raises a <see cref="RGBTemplateUpdatedEvent"/>.
    /// </summary>
    /// <param name="rgbTemplate">Template string for RGB format.</param>
    public void UpdateRGBTemplate(string rgbTemplate)
    {
        RGBTemplate = rgbTemplate;
        AddEvent(new RGBTemplateUpdatedEvent());
    }

    /// <summary>
    /// Updates the floating-point RGB template and raises a <see cref="RGBFloatTemplateUpdatedEvent"/>.
    /// </summary>
    /// <param name="rgbFloatTemplate">Template string for floating-point RGB format.</param>
    public void UpdateRGBFloatTemplate(string rgbFloatTemplate)
    {
        RGBFloatTemplate = rgbFloatTemplate;
        AddEvent(new RGBFloatTemplateUpdatedEvent());
    }

    /// <summary>
    /// Updates the HSV template and raises a <see cref="HSVTemplateUpdatedEvent"/>.
    /// </summary>
    /// <param name="hsvTemplate">Template string for HSV format.</param>
    public void UpdateHSVTemplate(string hsvTemplate)
    {
        HSVTemplate = hsvTemplate;
        AddEvent(new HSVTemplateUpdatedEvent());
    }

    /// <summary>
    /// Updates the HSL template and raises a <see cref="HSLTemplateUpdatedEvent"/>.
    /// </summary>
    /// <param name="hslTemplate">Template string for HSL format.</param>
    public void UpdateHSLTemplate(string hslTemplate)
    {
        HSLTemplate = hslTemplate;
        AddEvent(new HSLTemplateUpdatedEvent());
    }

    /// <summary>
    /// Updates the long integer color output template and raises a <see cref="LongTemplateUpdatedEvent"/>.
    /// </summary>
    /// <param name="longTemplate">Template string for long integer format.</param>
    public void UpdateLongTemplate(string longTemplate)
    {
        LongTemplate = longTemplate;
        AddEvent(new LongTemplateUpdatedEvent());
    }

    #endregion
}
