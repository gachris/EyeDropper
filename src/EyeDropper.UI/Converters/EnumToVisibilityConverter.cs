using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EyeDropper.UI.Converters;

/// <summary>
/// Converts between an enum value and <see cref="Visibility"/>, based on whether the enum value matches
/// the provided parameter string.
/// </summary>
public class EnumToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converts an enum value to a <see cref="Visibility"/> value. Returns
    /// <see cref="Visibility.Visible"/> if the enum value (as string) matches the <paramref name="parameter"/>,
    /// otherwise returns <see cref="Visibility.Collapsed"/>. If <paramref name="value"/> or
    /// <paramref name="parameter"/> is null, or if parsing fails, returns
    /// <see cref="DependencyProperty.UnsetValue"/>.
    /// </summary>
    /// <param name="value">The enum value from the binding source.</param>
    /// <param name="targetType">The type of the binding target property (expected to be <see cref="Visibility"/>).</param>
    /// <param name="parameter">
    /// A <see cref="string"/> representing the enum name to compare against <paramref name="value"/>.
    /// </param>
    /// <param name="culture">The culture to use in the converter (ignored).</param>
    /// <returns>
    /// <see cref="Visibility.Visible"/> if the parsed enum equals <paramref name="value"/>;
    /// <see cref="Visibility.Collapsed"/> if it does not match;
    /// otherwise <see cref="DependencyProperty.UnsetValue"/>.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return DependencyProperty.UnsetValue;

        if (parameter is not string enumString)
            return DependencyProperty.UnsetValue;

        Type enumType = value.GetType();
        if (!enumType.IsEnum)
            return DependencyProperty.UnsetValue;

        try
        {
            var enumValue = Enum.Parse(enumType, enumString, ignoreCase: true);
            return enumValue.Equals(value)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Converts a <see cref="Visibility"/> value back to the enum value it represents,
    /// based on the provided <paramref name="parameter"/>. Only converts when
    /// <paramref name="value"/> is <see cref="Visibility.Visible"/>; otherwise returns
    /// <see cref="DependencyProperty.UnsetValue"/>.
    /// </summary>
    /// <param name="value">The <see cref="Visibility"/> value from the binding target.</param>
    /// <param name="targetType">The type to convert back to (an enum type or nullable enum).</param>
    /// <param name="parameter">
    /// A <see cref="string"/> representing the enum name to return when <paramref name="value"/> is Visible.
    /// </param>
    /// <param name="culture">The culture to use in the converter (ignored).</param>
    /// <returns>
    /// The enum value represented by <paramref name="parameter"/> if <paramref name="value"/> is Visible;
    /// otherwise <see cref="DependencyProperty.UnsetValue"/>.
    /// </returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Visibility visibility || parameter is not string enumString)
            return DependencyProperty.UnsetValue;

        if (visibility != Visibility.Visible)
            return DependencyProperty.UnsetValue;

        Type enumType = Nullable.GetUnderlyingType(targetType) ?? targetType;
        if (!enumType.IsEnum)
            return DependencyProperty.UnsetValue;

        try
        {
            return Enum.Parse(enumType, enumString, ignoreCase: true);
        }
        catch
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
