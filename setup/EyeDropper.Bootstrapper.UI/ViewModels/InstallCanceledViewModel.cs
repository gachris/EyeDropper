using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Constants;
using DevToolbox.Core.Contracts;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel displayed when the user cancels an installation.
/// Provides options to close the application or retry installation.
/// </summary>
public partial class InstallCanceledViewModel : ObservableObject, INavigationViewModelAware
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;
    private readonly IWpfBootstrapperApplicationManager _bootstrapperApplicationManager;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether navigation back is allowed from this view.
    /// Always <c>false</c> in this scenario.
    /// </summary>
    public bool CanGoBack => false;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallCanceledViewModel"/> class.
    /// </summary>
    /// <param name="navigationService">
    /// The navigation service used to navigate between application pages.
    /// </param>
    /// <param name="bootstrapperApplicationManager">
    /// The application manager used to control navigation and application close.
    /// </param>
    public InstallCanceledViewModel(
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
    /// Closes the application when the user invokes the Close command.
    /// </summary>
    [RelayCommand]
    private void Close()
    {
        _bootstrapperApplicationManager.Close();
    }

    /// <summary>
    /// Retries installation by navigating to the install details page and clearing the history.
    /// </summary>
    [RelayCommand]
    private void Install()
    {
        _bootstrapperApplicationManager.ResetState();
    }

    #endregion
}
