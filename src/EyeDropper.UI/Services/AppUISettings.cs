using DevToolbox.Wpf.Media;
using EyeDropper.Application.Contracts;
using EyeDropper.UI.Contracts;

namespace EyeDropper.UI.Services;

/// <summary>
/// Manages application UI settings such as theme, persisting them via local settings.
/// </summary>
public class AppUISettings : IAppUISettings
{
    #region Fields/Consts

    private const string SettingsKey = "AppBackgroundRequestedTheme";

    private readonly ILocalSettingsService _localSettingsService;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the currently applied <see cref="ElementTheme"/>.
    /// </summary>
    public ElementTheme Theme { get; private set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="AppUISettings"/> class.
    /// </summary>
    /// <param name="localSettingsService">
    /// The service used to read and write local application settings.
    /// </param>
    public AppUISettings(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService
            ?? throw new ArgumentNullException(nameof(localSettingsService));
    }

    #endregion

    #region Methods

    /// <summary>
    /// Asynchronously initializes UI settings by loading persisted values and applying them.
    /// </summary>
    /// <returns>A <see cref="Task"/> that completes when initialization is finished.</returns>
    public async Task InitializeAsync()
    {
        // Enable dynamic text scaling across the application.
        FontSizeManager.TextScaleEnabled = true;

        // Read the persisted theme name from local settings.
        var themeName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        // Attempt to parse the stored string to an ElementTheme; default to WindowsDefault on failure.
        if (!Enum.TryParse<ElementTheme>(themeName, out var cacheTheme))
        {
            cacheTheme = ElementTheme.WindowsDefault;
        }

        // Apply and expose the loaded theme.
        Theme = cacheTheme;
        ThemeManager.RequestedTheme = Theme;
    }

    /// <summary>
    /// Asynchronously sets and persists the application theme.
    /// </summary>
    /// <param name="appTheme">The <see cref="ElementTheme"/> to apply.</param>
    /// <returns>A <see cref="Task"/> that completes when the theme has been set and saved.</returns>
    public async Task SetThemeAsync(ElementTheme appTheme)
    {
        // Save the selected theme to local settings.
        await _localSettingsService.SaveSettingAsync(SettingsKey, appTheme.ToString());

        // Apply and expose the new theme.
        Theme = appTheme;
        ThemeManager.RequestedTheme = Theme;
    }

    #endregion
}
