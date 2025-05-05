using EyeDropper.Bootstrapper.UI.Contracts;

namespace EyeDropper.Bootstrapper.UI.Services;

/// <summary>
/// Provides a mapping between string keys and page types for use in navigation.
/// </summary>
public class PageService : IPageService
{
    #region Fields/Consts

    /// <summary>
    /// Dictionary holding the mapping between page keys and their corresponding types.
    /// </summary>
    private readonly Dictionary<string, Type> _pages = [];

    #endregion

    #region Methods

    /// <summary>
    /// Retrieves the page <see cref="Type"/> associated with the specified key.
    /// </summary>
    /// <param name="key">The key representing the page.</param>
    /// <returns>The <see cref="Type"/> associated with the key.</returns>
    /// <exception cref="ArgumentException">Thrown if the key is not found in the page dictionary.</exception>
    public Type GetPageType(string key)
    {
        Type? pageType;
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }

        return pageType;
    }

    /// <summary>
    /// Adds a new page key and type mapping to the service.
    /// </summary>
    /// <param name="key">The key to associate with the page type.</param>
    /// <param name="type">The type of the page to register.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if the key is already registered, or if the type is already associated with another key.
    /// </exception>
    public void Configure(string key, Type type)
    {
        lock (_pages)
        {
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }

    #endregion
}
