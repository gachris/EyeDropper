namespace EyeDropper.Application.Services;

/// <summary>
/// Configuration options for the <see cref="LocalSettingsService"/>,
/// specifying the folders and filenames for local settings storage.
/// </summary>
public class LocalSettingsOptions
{
    /// <summary>
    /// Optional subfolder under the user's LocalApplicationData directory
    /// in which to create the application data folder.
    /// If null, the default folder ("EyeDropper/ApplicationData") is used.
    /// </summary>
    public string? ApplicationDataFolder { get; set; }

    /// <summary>
    /// Optional filename for the local settings JSON file.
    /// If null, the default filename ("LocalSettings.json") is used.
    /// </summary>
    public string? LocalSettingsFile { get; set; }
}