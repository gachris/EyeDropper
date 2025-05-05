using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Contracts;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel displayed when a downgrade of the application version is detected.
/// </summary>
public partial class DowngradeDetectedViewModel : ObservableObject, IViewModelAware
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
    /// Initializes a new instance of the <see cref="DowngradeDetectedViewModel"/> class.
    /// </summary>
    /// <param name="bootstrapperApplicationManager">
    /// The application manager used to control navigation and application close.
    /// </param>
    public DowngradeDetectedViewModel(IWpfBootstrapperApplicationManager bootstrapperApplicationManager)
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
    /// Closes the application when the user invokes the Close command.
    /// </summary>
    [RelayCommand]
    private void Close()
    {
        _bootstrapperApplicationManager.Close();
    }

    #endregion
}