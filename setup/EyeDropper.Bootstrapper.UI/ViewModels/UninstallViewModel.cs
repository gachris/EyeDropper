using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Constants;
using EyeDropper.Bootstrapper.UI.Contracts;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel for initiating the uninstallation process.
/// </summary>
public partial class UninstallViewModel : ObservableObject, IViewModelAware
{
    #region Fields/Consts

    /// <summary>
    /// Service used for navigating between pages.
    /// </summary>
    private readonly INavigationService _navigationService;

    /// <summary>
    /// Manages bootstrapper application lifecycle and triggers uninstallation.
    /// </summary>
    private readonly IWpfBootstrapperApplicationManager _bootstrapperApplicationManager;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether navigation back is allowed from this view.
    /// Always <c>false</c> on the uninstall initiation page.
    /// </summary>
    public bool CanGoBack => false;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UninstallViewModel"/> class.
    /// </summary>
    /// <param name="navigationService">
    /// The navigation service used to transition to the progress page.
    /// </param>
    /// <param name="bootstrapperApplicationManager">
    /// The application manager used to control uninstallation and application close.
    /// </param>
    public UninstallViewModel(
        INavigationService navigationService,
        IWpfBootstrapperApplicationManager bootstrapperApplicationManager)
    {
        _navigationService = navigationService;
        _bootstrapperApplicationManager = bootstrapperApplicationManager;
    }

    #endregion

    #region Navigation Methods

    /// <summary>
    /// Called when the view is navigated to.
    /// </summary>
    public void OnNavigated()
    {
    }

    /// <summary>
    /// Called when navigating away from this view.
    /// </summary>
    public void OnNavigatedAway()
    {
    }

    #endregion

    #region Relay Commands

    /// <summary>
    /// Starts the uninstallation process and navigates to the progress page.
    /// </summary>
    [RelayCommand]
    private async Task Uninstall()
    {
        await _navigationService.NavigateToAsync(PageKeys.UninstallProgressPage);
    }

    /// <summary>
    /// Cancels the uninstallation process and closes the bootstrapper.
    /// </summary>
    [RelayCommand]
    private void Cancel()
    {
        _bootstrapperApplicationManager.Close();
    }

    #endregion
}
