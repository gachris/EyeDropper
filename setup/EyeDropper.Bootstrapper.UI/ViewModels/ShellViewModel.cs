using System.Windows.Controls;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Constants;
using EyeDropper.Bootstrapper.UI.Contracts;
using WixToolset.BootstrapperApplicationApi;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel for the shell (root) view in the WiX Bootstrapper Application UI.
/// </summary>
public partial class ShellViewModel : ObservableObject
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;
    private readonly IWpfBootstrapperApplicationManager _bootstrapperApplicationManager;
    private IViewModelAware? _currentPageViewModel;
    private bool _shouldAddBackEntry;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether navigation can go back to the previous page.
    /// </summary>
    public bool CanGoBack => CurrentPageViewModel?.CanGoBack ?? false;

    /// <summary>
    /// Gets or sets the current page's view model.
    /// </summary>
    public IViewModelAware? CurrentPageViewModel
    {
        get => _currentPageViewModel;
        private set
        {
            _currentPageViewModel = value;
            OnPropertyChanged(nameof(CurrentPageViewModel));
            OnPropertyChanged(nameof(CanGoBack));
            GoBackCommand.NotifyCanExecuteChanged();
        }
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
    /// Subscribes to navigation and bootstrapper events and triggers initial navigation if waiting state.
    /// </summary>
    /// <param name="navigationService">The navigation service to use for page transitions.</param>
    /// <param name="bootstrapperApplicationManager">The bootstrapper manager for application lifecycle actions.</param>
    public ShellViewModel(
        INavigationService navigationService,
        IWpfBootstrapperApplicationManager bootstrapperApplicationManager)
    {
        _navigationService = navigationService;
        _bootstrapperApplicationManager = bootstrapperApplicationManager;

        navigationService.Navigated += NavigationService_Navigated;
        bootstrapperApplicationManager.OnActionRequested += BootstrapperApplicationManager_ActionRequested;
        bootstrapperApplicationManager.OnActionCompleted += BootstrapperApplicationManager_ActionCompleted;

        if (_bootstrapperApplicationManager.InstallationState is InstallationState.Waiting)
        {
            BootstrapperApplicationManager_ActionRequested(_bootstrapperApplicationManager, new ActionRequestedEventArgs(false));
        }
    }

    #region Methods

    #endregion

    #region Relay Commands

    /// <summary>
    /// Command to navigate back to the previous page.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private async Task GoBack()
    {
        await _navigationService.GoBackAsync();
    }

    #endregion

    #region Events Subscriptions

    /// <summary>
    /// Handles navigation events and updates the current page view model.
    /// Adds back entry if flagged.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">Navigation event arguments.</param>
    private async void NavigationService_Navigated(object sender, NavigationEventArgs e)
    {
        if (e.Content is Page page && page.DataContext is IViewModelAware viewModel)
        {
            CurrentPageViewModel = viewModel;
        }

        if (_shouldAddBackEntry)
        {
            _shouldAddBackEntry = false;
            await _navigationService.AddBackEntryAsync(PageKeys.InstallPage);
        }
    }

    /// <summary>
    /// Reacts to bootstrapper action requests and navigates to the appropriate page.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">Action requested event arguments.</param>
    private async void BootstrapperApplicationManager_ActionRequested(object? sender, ActionRequestedEventArgs e)
    {
        e.Handled = true;

        if (e.IsRetry)
        {
            _shouldAddBackEntry = true;
            await _navigationService.NavigateToAsync(PageKeys.InstallDetailsPage, true);
        }
        else if (_bootstrapperApplicationManager.Downgrade)
        {
            await _navigationService.NavigateToAsync(PageKeys.DowngradeDetectedPage);
        }
        else if (_bootstrapperApplicationManager.InstallationState is InstallationState.Failed)
        {
            await _navigationService.NavigateToAsync(PageKeys.ErrorPage);
        }
        else if (_bootstrapperApplicationManager.Action is LaunchAction.Uninstall)
        {
            if (_bootstrapperApplicationManager.Display is Display.Full)
            {
                await _navigationService.NavigateToAsync(PageKeys.UninstallPage);
            }
            else
            {
                await _navigationService.NavigateToAsync(PageKeys.UninstallProgressPage);
            }
        }
        else
        {
            if (_bootstrapperApplicationManager.Display is Display.Full)
            {
                await _navigationService.NavigateToAsync(PageKeys.InstallPage);
            }
            else
            {
                await _navigationService.NavigateToAsync(PageKeys.InstallProgressPage);
            }
        }
    }

    /// <summary>
    /// Reacts to bootstrapper action completion and navigates to success, cancellation, or error pages.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">Action completed event arguments.</param>
    private async void BootstrapperApplicationManager_ActionCompleted(object? sender, ActionCompletedEventArgs e)
    {
        if (e.AutoClose)
        {
            return;
        }

        if (_bootstrapperApplicationManager.Canceled)
        {
            if (_bootstrapperApplicationManager.PlannedAction is LaunchAction.Uninstall)
            {
                await _navigationService.NavigateToAsync(PageKeys.UninstallCanceledPage);
            }
            else
            {
                await _navigationService.NavigateToAsync(PageKeys.InstallCanceledPage);
            }

            return;
        }

        if (_bootstrapperApplicationManager.InstallationState is InstallationState.Failed)
        {
            if (e.ErrorType is ErrorType.Elevate)
            {
                await _navigationService.NavigateToAsync(PageKeys.ElevatedErrorPage);
            }
            else
            {
                await _navigationService.NavigateToAsync(PageKeys.ErrorPage);
            }

            return;
        }

        if (_bootstrapperApplicationManager.PlannedAction is LaunchAction.Uninstall)
        {
            await _navigationService.NavigateToAsync(PageKeys.UninstallSuccessfulPage);
        }
        else
        {
            await _navigationService.NavigateToAsync(PageKeys.InstallSuccessfulPage);
        }
    }

    #endregion
}
