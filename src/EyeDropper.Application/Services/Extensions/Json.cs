using Newtonsoft.Json;

namespace EyeDropper.Application.Services.Extensions;

/// <summary>
/// Provides asynchronous JSON serialization and deserialization helpers
/// leveraging <see cref="JsonConvert"/> internally.
/// </summary>
internal static class Json
{
    /// <summary>
    /// Deserializes the specified JSON string into an instance of <typeparamref name="T"/>
    /// on a background thread.
    /// </summary>
    /// <typeparam name="T">The target type into which to deserialize the JSON.</typeparam>
    /// <param name="value">The JSON string to deserialize.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the deserialized object of type <typeparamref name="T"/>, or null if deserialization fails.
    /// </returns>
    public static async Task<T?> ToObjectAsync<T>(string value)
    {
        return await Task.Run(() => JsonConvert.DeserializeObject<T>(value));
    }

    /// <summary>
    /// Serializes the specified object to a JSON string on a background thread.
    /// </summary>
    /// <param name="value">The object to serialize to JSON. Can be null.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the JSON string representation of the provided object.
    /// </returns>
    public static async Task<string> StringifyAsync(object? value)
    {
        return await Task.Run(() => JsonConvert.SerializeObject(value));
    }
}