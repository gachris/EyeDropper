using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Constants;
using EyeDropper.Bootstrapper.UI.Contracts;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel for the initial install page, displaying version information and navigation actions.
/// </summary>
public partial class InstallViewModel : ObservableObject, IViewModelAware
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;
    private readonly IWpfBootstrapperApplicationManager _bootstrapperApplicationManager;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether navigation back is allowed from this view.
    /// Always <c>false</c> on the initial install page.
    /// </summary>
    public bool CanGoBack => false;

    /// <summary>
    /// Gets the version of the application being installed.
    /// </summary>
    public string? Version { get; }

    /// <summary>
    /// Gets the version of the application already installed on the system, if any.
    /// </summary>
    public string? ExistingVersion { get; }

    /// <summary>
    /// Gets a value indicating whether an existing installation was detected.
    /// </summary>
    public bool HasExistingVersion { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallViewModel"/> class.
    /// </summary>
    /// <param name="navigationService">The navigation service for page transitions.</param>
    /// <param name="bootstrapperApplicationManager">
    /// The application manager providing install and existing version information.
    /// </param>
    public InstallViewModel(
        INavigationService navigationService,
        IWpfBootstrapperApplicationManager bootstrapperApplicationManager)
    {
        _navigationService = navigationService;
        _bootstrapperApplicationManager = bootstrapperApplicationManager;

        Version = _bootstrapperApplicationManager.Version;
        ExistingVersion = _bootstrapperApplicationManager.ExistingVersion;
        HasExistingVersion = !string.IsNullOrEmpty(ExistingVersion);
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
    /// Navigates to the installation details page.
    /// </summary>
    [RelayCommand]
    private async Task Next()
    {
        await _navigationService.NavigateToAsync(PageKeys.InstallDetailsPage);
    }

    /// <summary>
    /// Cancels the installation process and closes the bootstrapper.
    /// </summary>
    [RelayCommand]
    private void Cancel()
    {
        _bootstrapperApplicationManager.Close();
    }

    #endregion
}
