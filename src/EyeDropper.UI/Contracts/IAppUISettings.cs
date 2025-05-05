using DevToolbox.Wpf.Media;

namespace EyeDropper.UI.Contracts;

/// <summary>
/// Defines methods and properties for application UI settings management.
/// </summary>
public interface IAppUISettings
{
    /// <summary>
    /// Gets the current application theme.
    /// </summary>
    ElementTheme Theme { get; }

    /// <summary>
    /// Initializes the UI settings, loading any persisted preferences.
    /// </summary>
    /// <returns>A <see cref="Task"/> that completes when initialization is finished.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Applies the specified theme to the application UI and persists the choice.
    /// </summary>
    /// <param name="appTheme">The <see cref="ElementTheme"/> to apply.</param>
    /// <returns>A <see cref="Task"/> that completes when the theme has been set.</returns>
    Task SetThemeAsync(ElementTheme appTheme);
}
