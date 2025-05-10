using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core.Contracts;
using DevToolbox.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Helpers;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel displayed when elevation fails due to insufficient permissions or other errors.
/// </summary>
public partial class ElevateErrorViewModel : ObservableObject, INavigationViewModelAware
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
    /// Initializes a new instance of the <see cref="ElevateErrorViewModel"/> class.
    /// </summary>
    /// <param name="bootstrapperApplicationManager">
    /// The application manager used to control navigation and application close, and to obtain the log file path.
    /// </param>
    public ElevateErrorViewModel(IWpfBootstrapperApplicationManager bootstrapperApplicationManager)
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
    /// Opens the bootstrapper log file in the default application.
    /// </summary>
    [RelayCommand]
    private void OpenLog()
    {
        ShellHelper.LaunchUrl(_bootstrapperApplicationManager.LogFilePath);
    }

    /// <summary>
    /// Closes the application when the user invokes the Close command.
    /// </summary>
    [RelayCommand]
    private void Close()
    {
        _bootstrapperApplicationManager.Close();
    }

    #endregion
}
