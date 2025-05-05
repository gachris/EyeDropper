namespace EyeDropper.Core.RuntimeOptions;

/// <summary>
/// Represents runtime options for the EyeDropper application.
/// Holds user-configurable settings and templates for color output.
/// </summary>
public interface IRuntimeOptionsState
{
    /// <summary>
    /// Gets a value indicating whether the captured color should be copied to the clipboard automatically.
    /// </summary>
    bool CopyToClipboard { get; }

    /// <summary>
    /// Gets the primary modifier key code used to activate the color picker.
    /// </summary>
    int PrimaryModifierKey { get; }

    /// <summary>
    /// Gets the secondary modifier key code used to activate the color picker.
    /// </summary>
    int SecondaryModifierKey { get; }

    /// <summary>
    /// Gets the tertiary modifier key code used to activate the color picker.
    /// </summary>
    int TertiaryModifierKey { get; }

    /// <summary>
    /// Gets the hotkey code to trigger the color capture.
    /// </summary>
    int HotKey { get; }

    /// <summary>
    /// Gets the selected color format enumeration value.
    /// </summary>
    int ColorFormat { get; }

    /// <summary>
    /// Gets the number of decimal places to use when formatting floating point color components.
    /// </summary>
    int Precision { get; }

    /// <summary>
    /// Gets the template string for HTML color output.
    /// </summary>
    string HTMLTemplate { get; }

    /// <summary>
    /// Gets the template string for hexadecimal color output.
    /// </summary>
    string HexTemplate { get; }

    /// <summary>
    /// Gets the template string for Delphi-style hexadecimal color output.
    /// </summary>
    string DelphiHexTemplate { get; }

    /// <summary>
    /// Gets the template string for VB-style hexadecimal color output.
    /// </summary>
    string VBHexTemplate { get; }

    /// <summary>
    /// Gets the template string for RGB color output.
    /// </summary>
    string RGBTemplate { get; }

    /// <summary>
    /// Gets the template string for floating-point RGB color output.
    /// </summary>
    string RGBFloatTemplate { get; }

    /// <summary>
    /// Gets the template string for HSV color output.
    /// </summary>
    string HSVTemplate { get; }

    /// <summary>
    /// Gets the template string for HSL color output.
    /// </summary>
    string HSLTemplate { get; }

    /// <summary>
    /// Gets the template string for long integer color output.
    /// </summary>
    string LongTemplate { get; }

    /// <summary>
    /// Clears all runtime option values to their defaults.
    /// </summary>
    void ClearEvents();

    /// <summary>
    /// Updates the copy-to-clipboard setting.
    /// </summary>
    /// <param name="copyToClipboard">True to enable automatic copying to clipboard; otherwise false.</param>
    void UpdateCopyToClipboard(bool copyToClipboard);

    /// <summary>
    /// Updates the primary modifier key code.
    /// </summary>
    /// <param name="primaryModifierKey">Key code for the primary modifier.</param>
    void UpdatePrimaryModifierKey(int primaryModifierKey);

    /// <summary>
    /// Updates the secondary modifier key code.
    /// </summary>
    /// <param name="secondaryModifierKey">Key code for the secondary modifier.</param>
    void UpdateSecondaryModifierKey(int secondaryModifierKey);

    /// <summary>
    /// Updates the tertiary modifier key code.
    /// </summary>
    /// <param name="tertiaryModifierKey">Key code for the tertiary modifier.</param>
    void UpdateTertiaryModifierKey(int tertiaryModifierKey);

    /// <summary>
    /// Updates the hotkey code for triggering the color capture.
    /// </summary>
    /// <param name="hotKey">Key code for the hotkey.</param>
    void UpdateHotKey(int hotKey);

    /// <summary>
    /// Updates the selected color format.
    /// </summary>
    /// <param name="colorFormat">Enumeration value representing the desired format.</param>
    void UpdateColorFormat(int colorFormat);

    /// <summary>
    /// Updates the precision level for floating point color components.
    /// </summary>
    /// <param name="precision">Number of decimal places to use.</param>
    void UpdatePrecision(int precision);

    /// <summary>
    /// Updates the HTML output template.
    /// </summary>
    /// <param name="htmlTemplate">Template string for HTML format.</param>
    void UpdateHTMLTemplate(string htmlTemplate);

    /// <summary>
    /// Updates the hexadecimal output template.
    /// </summary>
    /// <param name="hexTemplate">Template string for hex format.</param>
    void UpdateHexTemplate(string hexTemplate);

    /// <summary>
    /// Updates the Delphi-style hexadecimal output template.
    /// </summary>
    /// <param name="delphiHexTemplate">Template string for Delphi hex format.</param>
    void UpdateDelphiHexTemplate(string delphiHexTemplate);

    /// <summary>
    /// Updates the VB-style hexadecimal output template.
    /// </summary>
    /// <param name="vbHexTemplate">Template string for VB hex format.</param>
    void UpdateVBHexTemplate(string vbHexTemplate);

    /// <summary>
    /// Updates the RGB output template.
    /// </summary>
    /// <param name="rgbTemplate">Template string for RGB format.</param>
    void UpdateRGBTemplate(string rgbTemplate);

    /// <summary>
    /// Updates the floating-point RGB output template.
    /// </summary>
    /// <param name="rgbFloatTemplate">Template string for floating-point RGB format.</param>
    void UpdateRGBFloatTemplate(string rgbFloatTemplate);

    /// <summary>
    /// Updates the HSV output template.
    /// </summary>
    /// <param name="hsvTemplate">Template string for HSV format.</param>
    void UpdateHSVTemplate(string hsvTemplate);

    /// <summary>
    /// Updates the HSL output template.
    /// </summary>
    /// <param name="hslTemplate">Template string for HSL format.</param>
    void UpdateHSLTemplate(string hslTemplate);

    /// <summary>
    /// Updates the long integer color output template.
    /// </summary>
    /// <param name="longTemplate">Template string for long integer format.</param>
    void UpdateLongTemplate(string longTemplate);
}
