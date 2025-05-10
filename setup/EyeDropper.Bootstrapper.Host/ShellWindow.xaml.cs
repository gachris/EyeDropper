using System.ComponentModel;
using System.Windows;
using DevToolbox.Core.Contracts;
using DevToolbox.Wpf.Windows;
using EyeDropper.Bootstrapper.Core;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.UI.ViewModels;
using WixToolset.BootstrapperApplicationApi;

namespace EyeDropper.Bootstrapper.Host;

/// <summary>
/// Represents the main shell window for the EyeDropper bootstrapper.
/// Hosts the installation UI and handles user interactions such as canceling the installation.
/// </summary>
public partial class ShellWindow : WindowEx
{
    #region Fields/Consts

    private readonly IWpfBootstrapperApplicationManager _bAManager;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ShellWindow"/> class.
    /// </summary>
    /// <param name="bAManager">The bootstrapper application manager.</param>
    /// <param name="navigationService">The navigation service used to switch views.</param>
    /// <param name="viewModel">The view model for the shell window.</param>
    public ShellWindow(
        IWpfBootstrapperApplicationManager bAManager,
        INavigationService navigationService,
        ShellViewModel viewModel)
    {
        _bAManager = bAManager;
        DataContext = viewModel;

        InitializeComponent();

        navigationService.Frame = MainFrame;

        Chrome.ResizeBorderThickness = new Thickness(0);
        Chrome.CaptionHeight = 44;
    }

    #region Methods

    /// <summary>
    /// Attempts to cancel the installation or uninstallation process.
    /// Displays a confirmation dialog when in full display mode.
    /// </summary>
    private void Cancel()
    {
        if (_bAManager.Canceled)
        {
            return;
        }

        var isUninstall = _bAManager.Action == LaunchAction.Uninstall;

        var msg = isUninstall
            ? Localization.Properties.Resources.Cancel_uninstallation_message
            : Localization.Properties.Resources.Cancel_installation_message;
        var title = isUninstall
            ? Localization.Properties.Resources.Cancel_uninstallation_title
            : Localization.Properties.Resources.Cancel_installation_title;

        if (_bAManager.Display == Display.Full)
        {
            var result = MessageBox.Show(
                Application.Current.MainWindow,
                msg,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result is MessageBoxResult.Yes)
            {
                _bAManager.Cancel(autoClose: true);
            }
        }
        else
        {
            _bAManager.Cancel(autoClose: true);
        }
    }

    #endregion

    #region Events Subscriptions

    /// <summary>
    /// Handles the <see cref="Window.Loaded"/> event and closes the splash screen.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The routed event arguments.</param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _bAManager.CloseSplashScreen();
    }

    /// <summary>
    /// Handles the <see cref="Window.Closing"/> event.
    /// If the installation is currently applying, prompts for cancellation.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="e">The cancel event arguments.</param>
    private void Window_Closing(object? sender, CancelEventArgs e)
    {
        if (_bAManager.InstallationState is not InstallationState.Applying)
            return;

        Cancel();

        if (_bAManager.Canceled)
            e.Cancel = true;
    }

    #endregion
}
