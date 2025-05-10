using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core.Contracts;
using DevToolbox.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Helpers;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel displayed when the installation completes successfully.
/// Provides commands to launch the installed application or close the bootstrapper.
/// </summary>
public partial class InstallSuccessfulViewModel : ObservableObject, INavigationViewModelAware
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
    /// Initializes a new instance of the <see cref="InstallSuccessfulViewModel"/> class.
    /// </summary>
    /// <param name="bootstrapperApplicationManager">
    /// The application manager used to control navigation and application close.
    /// </param>
    public InstallSuccessfulViewModel(IWpfBootstrapperApplicationManager bootstrapperApplicationManager)
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
    /// Launches the installed EyeDropper application and closes the bootstrapper.
    /// </summary>
    [RelayCommand]
    private void LaunchApp()
    {
        var executablePath = Path.Combine(_bootstrapperApplicationManager.InstallDirectory, "EyeDropper.exe");
        ShellHelper.LaunchUrl(executablePath);

        _bootstrapperApplicationManager.Close();
    }

    /// <summary>
    /// Closes the bootstrapper application.
    /// </summary>
    [RelayCommand]
    private void Close()
    {
        _bootstrapperApplicationManager.Close();
    }

    #endregion
}
