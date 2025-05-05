namespace EyeDropper.Application.Contracts;

/// <summary>
/// Defines methods for reading and persisting local application settings.
/// </summary>
public interface ILocalSettingsService
{
    /// <summary>
    /// Reads the setting value associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The expected type of the stored value.</typeparam>
    /// <param name="key">The unique key identifying the setting.</param>
    /// <returns>
    /// A task that represents the asynchronous read operation. The task result contains the value
    /// if found and deserialized to <typeparamref name="T"/>, or null if not present.
    /// </returns>
    Task<T?> ReadSettingAsync<T>(string key);

    /// <summary>
    /// Saves the specified value under the given key in the local settings store.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize and save.</typeparam>
    /// <param name="key">The unique key under which to store the setting.</param>
    /// <param name="value">The value to serialize and persist.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveSettingAsync<T>(string key, T value);
}