using System.Windows.Controls;
using System.Windows.Navigation;

namespace EyeDropper.Bootstrapper.UI.Contracts;

/// <summary>
/// Provides navigation functionality for managing pages within a Frame.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Occurs when the content that is being navigated to has been found, and is available
    /// from the System.Windows.Controls.ContentControl.Content property, although it
    /// may not have completed loading.
    /// </summary>
    event NavigatedEventHandler Navigated;

    /// <summary>
    /// Gets or sets the Frame used for navigation.
    /// </summary>
    Frame? Frame { get; set; }

    /// <summary>
    /// Gets a value indicating whether it is possible to navigate back.
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    /// Adds an entry to the back navigation history stack.
    /// </summary>
    /// <param name="pageKey">The key representing the page to add to the back stack.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the entry was added successfully; otherwise, false.</returns>
    Task<bool> AddBackEntryAsync(string pageKey);

    /// <summary>
    /// Navigates back to the previous page in the navigation history, if possible.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if navigation was successful; otherwise, false.</returns>
    Task<bool> GoBackAsync();

    /// <summary>
    /// Navigates to the specified page.
    /// </summary>
    /// <param name="pageKey">The key identifying the page to navigate to.</param>
    /// <param name="parameter">An optional parameter to pass to the target page.</param>
    /// <param name="clearNavigation">If true, clears the existing navigation stack before navigating.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if navigation was successful; otherwise, false.</returns>
    Task<bool> NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false);
}