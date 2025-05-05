using System.Windows.Input;
using DevToolbox.Wpf.Controls;
using EyeDropper.Localization.Properties;

namespace EyeDropper.UI.Constants;

/// <summary>
/// Contains constant collections for various application settings,
/// including numeric precisions, modifier keys, hotkeys, and supported color formats.
/// </summary>
public static class SettingsConstants
{
    /// <summary>
    /// Gets the supported numeric precisions for color component values (1 through 11).
    /// </summary>
    public static readonly IEnumerable<int> Precisions = Enumerable.Range(1, 11);

    /// <summary>
    /// Gets the set of modifier keys that can be used in keyboard shortcuts.
    /// </summary>
    public static readonly IEnumerable<ModifierKeys> ModifierKeys =
        Enum.GetValues(typeof(ModifierKeys)).Cast<ModifierKeys>();

    /// <summary>
    /// Gets the set of primary keys that can be used in keyboard shortcuts.
    /// </summary>
    public static readonly IEnumerable<Key> HotKeys =
        Enum.GetValues(typeof(Key)).Cast<Key>();

    /// <summary>
    /// Gets the mapping between <see cref="ColorFormat"/> values and their localized display names.
    /// </summary>
    public static readonly IDictionary<ColorFormat, string> ColorFormats =
        new Dictionary<ColorFormat, string>
        {
            { ColorFormat.HTML, Resources.HTML },
            { ColorFormat.Hex, Resources.Hex },
            { ColorFormat.DelphiHex, Resources.Delphi_Hex },
            { ColorFormat.VBHex, Resources.VB_Hex },
            { ColorFormat.RGB, Resources.RGB },
            { ColorFormat.RGBFloat, Resources.RGB_Float },
            { ColorFormat.HSV, Resources.HSV },
            { ColorFormat.HSL, Resources.HSL },
            { ColorFormat.Long, Resources.Long }
        };
}