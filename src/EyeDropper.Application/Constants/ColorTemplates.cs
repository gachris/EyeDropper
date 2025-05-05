namespace EyeDropper.Application.Constants;

/// <summary>
/// Defines the available color output templates for various formats.
/// </summary>
public static class ColorTemplates
{
    /// <summary>
    /// Gets the collection of HTML templates, including formats with and without a leading '#'.
    /// </summary>
    public static readonly IReadOnlyCollection<string> HTMLTemplates =
    [
        HTML.NumberSignRGB,
        HTML.RGB,
        HTML.NumberSignLowerRGB,
        HTML.LowerRGB,
    ];

    /// <summary>
    /// Gets the collection of hexadecimal templates, including both RGB and BGR orders.
    /// </summary>
    public static readonly IReadOnlyCollection<string> HexTemplates =
    [
        Hex.BGR,
        Hex.RGB,
        Hex.LowerBGR,
        Hex.LowerRGB,
    ];

    /// <summary>
    /// Gets the collection of Delphi-style hexadecimal templates.
    /// </summary>
    public static readonly IReadOnlyCollection<string> DelphiHexTemplates =
    [
        DelphiHex.BGR,
        DelphiHex.LowerBGR,
    ];

    /// <summary>
    /// Gets the collection of VB-style hexadecimal templates.
    /// </summary>
    public static readonly IReadOnlyCollection<string> VBHexTemplates =
    [
        VBHex.BGR,
    ];

    /// <summary>
    /// Gets the collection of integer RGB templates (comma-, newline-, and tab-separated).
    /// </summary>
    public static readonly IReadOnlyCollection<string> RGBTemplates =
    [
        RGB.CommaSeparated,
        RGB.NewLineSeparated,
        RGB.TabLineSeparated,
    ];

    /// <summary>
    /// Gets the collection of floating-point RGB templates (comma-, newline-, and tab-separated).
    /// </summary>
    public static readonly IReadOnlyCollection<string> RGBFloatTemplates =
    [
        RGBFloat.CommaSeparated,
        RGBFloat.NewLineSeparated,
        RGBFloat.TabLineSeparated,
    ];

    /// <summary>
    /// Gets the collection of HSV templates (comma-, newline-, and tab-separated).
    /// </summary>
    public static readonly IReadOnlyCollection<string> HSVTemplates =
    [
        HSV.CommaSeparated,
        HSV.NewLineSeparated,
        HSV.TabLineSeparated,
    ];

    /// <summary>
    /// Gets the collection of HSL templates (comma-, newline-, and tab-separated).
    /// </summary>
    public static readonly IReadOnlyCollection<string> HSLTemplates =
    [
        HSL.CommaSeparated,
        HSL.NewLineSeparated,
        HSL.TabLineSeparated,
    ];

    /// <summary>
    /// Gets the collection of long integer templates.
    /// </summary>
    public static readonly IReadOnlyCollection<string> LongTemplates =
    [
        Long.Number,
    ];

    /// <summary>
    /// HTML color template constants.
    /// </summary>
    public static class HTML
    {
        /// <summary>
        /// Template with leading '#' and uppercase hex digits.
        /// </summary>
        public const string NumberSignRGB = "#(R)(G)(B)";

        /// <summary>
        /// Template without leading '#' and uppercase hex digits.
        /// </summary>
        public const string RGB = "(R)(G)(B)";

        /// <summary>
        /// Template with leading '#' and lowercase hex digits.
        /// </summary>
        public const string NumberSignLowerRGB = "#(r)(g)(b)";

        /// <summary>
        /// Template without leading '#' and lowercase hex digits.
        /// </summary>
        public const string LowerRGB = "(r)(g)(b)";
    }

    /// <summary>
    /// Standard hexadecimal template constants.
    /// </summary>
    public static class Hex
    {
        /// <summary>
        /// Macro representing BGR order with uppercase digits.
        /// </summary>
        public const string BGR = "0x(B)(G)(R)";

        /// <summary>
        /// Macro representing RGB order with uppercase digits.
        /// </summary>
        public const string RGB = "0x(R)(G)(B)";

        /// <summary>
        /// Macro representing BGR order with lowercase digits.
        /// </summary>
        public const string LowerBGR = "0x(b)(g)(r)";

        /// <summary>
        /// Macro representing RGB order with lowercase digits.
        /// </summary>
        public const string LowerRGB = "0x(r)(g)(b)";
    }

    /// <summary>
    /// Delphi-style hexadecimal template constants.
    /// </summary>
    public static class DelphiHex
    {
        /// <summary>
        /// Delphi-style BGR template with uppercase digits.
        /// </summary>
        public const string BGR = "$00(B)(G)(R)";

        /// <summary>
        /// Delphi-style BGR template with lowercase digits.
        /// </summary>
        public const string LowerBGR = "$00(b)(g)(r)";
    }

    /// <summary>
    /// VB-style hexadecimal template constants.
    /// </summary>
    public static class VBHex
    {
        /// <summary>
        /// VB-style BGR template with leading '&H' and trailing '&'.
        /// </summary>
        public const string BGR = "&H00(B)(G)(R)&";
    }

    /// <summary>
    /// Integer RGB template constants.
    /// </summary>
    public static class RGB
    {
        /// <summary>
        /// Comma-separated RGB values.
        /// </summary>
        public const string CommaSeparated = "(r), (g), (b)";

        /// <summary>
        /// Newline-separated RGB values.
        /// </summary>
        public const string NewLineSeparated = "(r)\\n(g)\\n(b)";

        /// <summary>
        /// Tab-separated RGB values.
        /// </summary>
        public const string TabLineSeparated = "(r)\\t(g)\\t(b)";
    }

    /// <summary>
    /// Floating-point RGB template constants.
    /// </summary>
    public static class RGBFloat
    {
        /// <summary>
        /// Comma-separated floating-point RGB values.
        /// </summary>
        public const string CommaSeparated = "(r), (g), (b)";

        /// <summary>
        /// Newline-separated floating-point RGB values.
        /// </summary>
        public const string NewLineSeparated = "(r)\\n(g)\\n(b)";

        /// <summary>
        /// Tab-separated floating-point RGB values.
        /// </summary>
        public const string TabLineSeparated = "(r)\\t(g)\\t(b)";
    }

    /// <summary>
    /// HSV template constants.
    /// </summary>
    public static class HSV
    {
        /// <summary>
        /// Comma-separated HSV values.
        /// </summary>
        public const string CommaSeparated = "(h), (s), (v)";

        /// <summary>
        /// Newline-separated HSV values.
        /// </summary>
        public const string NewLineSeparated = "(h)\\n(s)\\n(v)";

        /// <summary>
        /// Tab-separated HSV values.
        /// </summary>
        public const string TabLineSeparated = "(h)\\t(s)\\t(v)";
    }

    /// <summary>
    /// HSL template constants.
    /// </summary>
    public static class HSL
    {
        /// <summary>
        /// Comma-separated HSL values.
        /// </summary>
        public const string CommaSeparated = "(h), (s), (l)";

        /// <summary>
        /// Newline-separated HSL values.
        /// </summary>
        public const string NewLineSeparated = "(h)\\n(s)\\n(l)";

        /// <summary>
        /// Tab-separated HSL values.
        /// </summary>
        public const string TabLineSeparated = "(h)\\t(s)\\t(l)";
    }

    /// <summary>
    /// Long integer template constants.
    /// </summary>
    public static class Long
    {
        /// <summary>
        /// Single long integer value template.
        /// </summary>
        public const string Number = "(long)";
    }
}