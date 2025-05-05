using System.Text;
using EyeDropper.Application.Contracts;
using Newtonsoft.Json;

namespace EyeDropper.Application.Services;

/// <summary>
/// Provides file-based I/O operations for reading from, saving to, and deleting JSON files.
/// Implements <see cref="IFileService"/> using the local file system.
/// </summary>
public class FileService : IFileService
{
    #region Methods

    /// <summary>
    /// Reads and deserializes JSON content from a file in the specified folder.
    /// </summary>
    /// <typeparam name="T">The type to which the JSON content will be deserialized.</typeparam>
    /// <param name="folderPath">The path to the folder containing the file.</param>
    /// <param name="fileName">The name of the file to read.</param>
    /// <returns>
    /// An instance of <typeparamref name="T"/> deserialized from the file's JSON content,
    /// or null if the file does not exist.
    /// </returns>
    public T? Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        return default;
    }

    /// <summary>
    /// Serializes the provided content as JSON and saves it to a file in the specified folder.
    /// </summary>
    /// <typeparam name="T">The type of the content to serialize.</typeparam>
    /// <param name="folderPath">The path to the folder where the file will be saved.</param>
    /// <param name="fileName">The name of the file to write.</param>
    /// <param name="content">The content object to serialize and save.</param>
    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    /// <summary>
    /// Deletes the specified file from the given folder if it exists.
    /// </summary>
    /// <param name="folderPath">The path to the folder containing the file.</param>
    /// <param name="fileName">The name of the file to delete.</param>
    public void Delete(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (!string.IsNullOrEmpty(fileName) && File.Exists(path))
        {
            File.Delete(path);
        }
    }

    #endregion
}