using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Contracts;
using EyeDropper.Localization.Properties;
using WixToolset.BootstrapperApplicationApi;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel for displaying uninstallation progress and handling cancellation.
/// </summary>
public partial class UninstallProgressViewModel : ObservableObject, IViewModelAware
{
    #region Fields/Consts

    private readonly IWpfBootstrapperApplicationManager _bootstrapperApplicationManager;

    /// <summary>
    /// Gets or sets the current uninstallation progress percentage (0–100).
    /// </summary>
    [ObservableProperty]
    private int _progress;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the Cancel command can execute.
    /// </summary>
    public bool CanCancel => !_bootstrapperApplicationManager.Canceled;

    /// <summary>
    /// Gets a value indicating whether the uninstallation has been canceled.
    /// </summary>
    public bool Canceled => _bootstrapperApplicationManager.Canceled;

    /// <summary>
    /// Gets a value indicating whether navigation back is allowed from this view.
    /// Always <c>false</c> in this scenario.
    /// </summary>
    public bool CanGoBack => false;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="UninstallProgressViewModel"/> class.
    /// Subscribes to progress and cancellation events.
    /// </summary>
    /// <param name="bootstrapperApplicationManager">
    /// The application manager used to monitor progress and handle cancellation.
    /// </param>
    public UninstallProgressViewModel(IWpfBootstrapperApplicationManager bootstrapperApplicationManager)
    {
        _bootstrapperApplicationManager = bootstrapperApplicationManager;
        _bootstrapperApplicationManager.OnCanceled += BootstrapperApplicationManager_OnCanceled;
        _bootstrapperApplicationManager.OnProgress += BootstrapperApplicationManager_Progress;
    }

    #endregion

    #region Navigation Methods

    /// <summary>
    /// Called when the view is navigated to.
    /// </summary>
    public void OnNavigated()
    {
        Progress = 0;

        OnPropertyChanged(nameof(Canceled));
        CancelCommand.NotifyCanExecuteChanged();

        _bootstrapperApplicationManager.Uninstall();
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
    /// Cancels the uninstallation process. Prompts for confirmation if in full display mode.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanCancel))]
    private void Cancel()
    {
        if (_bootstrapperApplicationManager.Canceled)
            return;

        if (_bootstrapperApplicationManager.Display is Display.Full)
        {
            var result = MessageBox.Show(
                Application.Current.MainWindow,
                Resources.Cancel_uninstallation_message,
                Resources.Cancel_uninstallation_title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;
        }

        _bootstrapperApplicationManager.Cancel();
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles the OnCanceled event to update UI bindings.
    /// </summary>
    private void BootstrapperApplicationManager_OnCanceled(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(Canceled));
        CancelCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Handles the OnProgress event to update the <see cref="Progress"/> property.
    /// </summary>
    /// <param name="sender">Event sender (ignored).</param>
    /// <param name="e">Progress event arguments containing the new progress value.</param>
    private void BootstrapperApplicationManager_Progress(object? sender, InstallationProgressEventArgs e)
    {
        Progress = e.Progress;
    }

    #endregion
}
