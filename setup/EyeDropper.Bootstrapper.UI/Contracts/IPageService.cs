namespace EyeDropper.Bootstrapper.UI.Contracts;

/// <summary>
/// Provides methods for configuring and retrieving page types by key for navigation purposes.
/// </summary>
public interface IPageService
{
    /// <summary>
    /// Gets the page <see cref="Type"/> associated with the specified key.
    /// </summary>
    /// <param name="key">The key representing the page.</param>
    /// <returns>The <see cref="Type"/> of the page.</returns>
    Type GetPageType(string key);

    /// <summary>
    /// Configures a mapping between a page key and a page <see cref="Type"/>.
    /// </summary>
    /// <param name="key">The key to associate with the page type.</param>
    /// <param name="type">The type of the page to associate with the key.</param>
    void Configure(string key, Type type);
}