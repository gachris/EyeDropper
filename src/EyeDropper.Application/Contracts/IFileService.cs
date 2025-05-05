namespace EyeDropper.Application.Contracts;

/// <summary>
/// Provides basic file operations for reading, saving, and deleting content from a specified folder and file.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Reads and deserializes content from the specified file in the given folder.
    /// </summary>
    /// <typeparam name="T">The expected type of the deserialized content.</typeparam>
    /// <param name="folderPath">The path of the folder containing the file.</param>
    /// <param name="fileName">The name of the file to read.</param>
    /// <returns>
    /// The deserialized content of type <typeparamref name="T"/>,
    /// or null if the file does not exist or cannot be read.
    /// </returns>
    T? Read<T>(string folderPath, string fileName);

    /// <summary>
    /// Serializes and saves the provided content to the specified file in the given folder.
    /// </summary>
    /// <typeparam name="T">The type of the content to serialize.</typeparam>
    /// <param name="folderPath">The path of the folder in which to save the file.</param>
    /// <param name="fileName">The name of the file to create or overwrite.</param>
    /// <param name="content">The content to serialize and save.</param>
    void Save<T>(string folderPath, string fileName, T content);

    /// <summary>
    /// Deletes the specified file from the given folder, if it exists.
    /// </summary>
    /// <param name="folderPath">The path of the folder containing the file.</param>
    /// <param name="fileName">The name of the file to delete.</param>
    void Delete(string folderPath, string fileName);
}