using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core.Contracts;
using DevToolbox.Core.Contracts;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel displayed when the uninstallation completes successfully.
/// Provides a command to dismiss the bootstrapper.
/// </summary>
public partial class UninstallSuccessfulViewModel : ObservableObject, INavigationViewModelAware
{
    #region Fields/Consts

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
    /// Initializes a new instance of the <see cref="UninstallSuccessfulViewModel"/> class.
    /// </summary>
    /// <param name="bootstrapperApplicationManager">
    /// The application manager used to control application close.
    /// </param>
    public UninstallSuccessfulViewModel(IWpfBootstrapperApplicationManager bootstrapperApplicationManager)
    {
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
    /// Dismisses the bootstrapper application when the user invokes the Dismiss command.
    /// </summary>
    [RelayCommand]
    private void Dismiss()
    {
        _bootstrapperApplicationManager.Close();
    }

    #endregion
}