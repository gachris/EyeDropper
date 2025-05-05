using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CommonServiceLocator;
using EyeDropper.Bootstrapper.UI.Contracts;

namespace EyeDropper.Bootstrapper.UI.Services;

/// <summary>
/// Provides an implementation of <see cref="INavigationService"/> for navigating between pages in a WPF application.
/// </summary>
public class NavigationService : INavigationService
{
    /// <summary>
    /// Represents a custom navigation state that supports dependency injection during navigation replay.
    /// </summary>
    [Serializable]
    private class DIContentState : CustomContentState
    {
        private readonly Type _pageType;
        private readonly IServiceLocator _serviceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DIContentState"/> class.
        /// </summary>
        /// <param name="pageType">The type of the page to navigate to.</param>
        /// <param name="serviceLocator">The service locator used to resolve page instances.</param>
        public DIContentState(Type pageType, IServiceLocator serviceLocator)
        {
            _pageType = pageType;
            _serviceLocator = serviceLocator;
        }

        /// <summary>
        /// Replays the navigation state by navigating to the resolved page instance.
        /// </summary>
        /// <param name="navigationService">The navigation service performing the navigation.</param>
        /// <param name="mode">The navigation mode.</param>
        public override void Replay(System.Windows.Navigation.NavigationService navigationService, NavigationMode mode)
        {
            var pageInstance = _serviceLocator.GetService(_pageType);
            if (pageInstance is not null && navigationService.Content != pageInstance)
            {
                navigationService.Navigate(pageInstance);
            }
        }
    }

    #region Fields/Consts

    private readonly IPageService _pageService;
    private readonly IServiceLocator _serviceLocator;
    private readonly List<FrameworkElement> _lastViews = [];
    private Frame? _frame;

    /// <summary>
    /// Occurs when navigation to a new content fragment is completed.
    /// </summary>
    public event NavigatedEventHandler? Navigated;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the <see cref="Frame"/> used for navigation.
    /// </summary>
    public Frame? Frame
    {
        get => _frame;
        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the navigation service can navigate back.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => _frame?.CanGoBack == true;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    /// <param name="pageService">Service that resolves page types from keys.</param>
    /// <param name="serviceLocator">Service locator for resolving page instances.</param>
    public NavigationService(IPageService pageService, IServiceLocator serviceLocator)
    {
        _pageService = pageService;
        _serviceLocator = serviceLocator;
    }

    #region Methods

    /// <summary>
    /// Registers navigation event handlers for the current <see cref="Frame"/>.
    /// </summary>
    private void RegisterFrameEvents()
    {
        if (_frame is not null)
        {
            _frame.Navigating += Frame_Navigating;
            _frame.Navigated += Frame_Navigated;
        }
    }

    /// <summary>
    /// Unregisters navigation event handlers from the current <see cref="Frame"/>.
    /// </summary>
    private void UnregisterFrameEvents()
    {
        if (_frame is not null)
        {
            _frame.Navigating -= Frame_Navigating;
            _frame.Navigated -= Frame_Navigated;
        }
    }

    /// <summary>
    /// Adds a back entry to the navigation stack using a page key.
    /// </summary>
    /// <param name="pageKey">The key identifying the page.</param>
    /// <returns>True if the back entry was added; otherwise, false.</returns>
    public async Task<bool> AddBackEntryAsync(string pageKey)
    {
        if (_frame?.NavigationService != null)
        {
            var pageType = _pageService.GetPageType(pageKey);

            _frame.NavigationService.AddBackEntry(new DIContentState(pageType, _serviceLocator));

            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }

    /// <summary>
    /// Navigates to the previous page in the navigation stack.
    /// </summary>
    /// <returns>True if navigation was successful; otherwise, false.</returns>
    public async Task<bool> GoBackAsync()
    {
        if (!CanGoBack)
            return await Task.FromResult(false);

        _frame.NavigationService.GoBack();

        return await Task.FromResult(true);
    }

    /// <summary>
    /// Navigates to the specified page using a page key, but skips navigation
    /// if the frame is already displaying that page type.
    /// </summary>
    /// <param name="pageKey">The key identifying the page to navigate to.</param>
    /// <param name="parameter">An optional parameter to pass to the page.</param>
    /// <param name="clearNavigation">Indicates whether the navigation history should be cleared.</param>
    /// <returns>True if navigation was performed (or you were already on that page); false if unable to navigate.</returns>
    public async Task<bool> NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        if (_frame is null)
            return false;

        var pageType = _pageService.GetPageType(pageKey);
        if (pageType is null)
            return false;

        var current = _frame.Content;
        if (current != null && current.GetType() == pageType)
            return true;

        var pageInstance = _serviceLocator.GetService(pageType);
        if (pageInstance is null)
            return false;

        var navigated = _frame.NavigationService.Navigate(pageInstance, parameter);

        if (clearNavigation && navigated)
        {
            while (_frame.CanGoBack)
                _frame.NavigationService.RemoveBackEntry();
        }

        return await Task.FromResult(navigated);
    }

    #endregion

    #region Events Subscriptions

    /// <summary>
    /// Handles the <see cref="Frame.Navigating"/> event to notify view models of navigation away.
    /// </summary>
    private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
    {
        var lastView = _lastViews.LastOrDefault();
        if (lastView?.DataContext is IViewModelAware vm)
        {
            vm.OnNavigatedAway();
        }

        if (lastView is not null)
        {
            _lastViews.Remove(lastView);
        }
    }

    /// <summary>
    /// Handles the <see cref="Frame.Navigated"/> event to notify view models of navigation and update view tracking.
    /// </summary>
    private void Frame_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Content is FrameworkElement fe)
        {
            if (fe.DataContext is IViewModelAware vm)
            {
                vm.OnNavigated();
            }

            _lastViews.Add(fe);
        }

        Navigated?.Invoke(sender, e);
    }

    #endregion
}