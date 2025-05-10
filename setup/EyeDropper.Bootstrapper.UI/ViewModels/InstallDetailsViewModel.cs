using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Constants;
using DevToolbox.Core.Contracts;
using EyeDropper.Bootstrapper.UI.Helpers;
using EyeDropper.Localization.Properties;

namespace EyeDropper.Bootstrapper.UI.ViewModels;

/// <summary>
/// ViewModel for the installation details page, allowing the user to select installation options and start installation.
/// </summary>
public partial class InstallDetailsViewModel : ObservableObject, INavigationViewModelAware
{
    #region Fields/Consts

    private readonly INavigationService _navigationService;
    private readonly IWpfBootstrapperApplicationManager _bootstrapperApplicationManager;
    private readonly string _defaultInstallFolder;
    private readonly bool _defaultCreateDesktopShortcut;
    private readonly bool _defaultCreateStartMenuShortcut;
    private readonly bool _defaultLaunchOnStartup;

    /// <summary>
    /// Gets or sets whether the user has agreed to the license terms.
    /// Triggers CanExecute re-evaluation of the Install command.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(InstallCommand))]
    private bool _agree;

    /// <summary>
    /// Gets or sets the selected installation folder path.
    /// Triggers CanExecute re-evaluation of the Install command.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(InstallCommand))]
    private string _installFolder = null!;

    /// <summary>
    /// Gets or sets whether to create a desktop shortcut after installation.
    /// </summary>
    [ObservableProperty]
    private bool _createDesktopShortcut = true;

    /// <summary>
    /// Gets or sets whether to create a start menu shortcut after installation.
    /// </summary>
    [ObservableProperty]
    private bool _createStartMenuShortcut = true;

    /// <summary>
    /// Gets or sets whether to launch the application on startup after installation.
    /// </summary>
    [ObservableProperty]
    private bool _launchOnStartup = true;

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether navigation back is allowed from this view.
    /// Always <c>true</c> to allow returning to the previous page.
    /// </summary>
    public bool CanGoBack => true;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallDetailsViewModel"/> class.
    /// </summary>
    /// <param name="navigationService">Service used for navigating between pages.</param>
    /// <param name="bootstrapperApplicationManager">Manager for bootstrapper application settings and lifecycle.</param>
    public InstallDetailsViewModel(
        INavigationService navigationService,
        IWpfBootstrapperApplicationManager bootstrapperApplicationManager)
    {
        _navigationService = navigationService;
        _bootstrapperApplicationManager = bootstrapperApplicationManager;

        _defaultInstallFolder = _bootstrapperApplicationManager.InstallDirectory;
        _defaultCreateDesktopShortcut = _bootstrapperApplicationManager.CreateDesktopShortcut;
        _defaultCreateStartMenuShortcut = _bootstrapperApplicationManager.CreateStartMenuShortcut;
        _defaultLaunchOnStartup = _bootstrapperApplicationManager.LaunchOnStartup;
    }

    #endregion

    #region Navigation Methods

    /// <summary>
    /// Called when the view is navigated to. Resets view state to defaults.
    /// </summary>
    public void OnNavigated()
    {
        Agree = false;
        InstallFolder = _defaultInstallFolder;
        CreateDesktopShortcut = _defaultCreateDesktopShortcut;
        CreateStartMenuShortcut = _defaultCreateStartMenuShortcut;
        LaunchOnStartup = _defaultLaunchOnStartup;
    }

    /// <summary>
    /// Called when navigating away from this view.
    /// </summary>
    public void OnNavigatedAway()
    {
    }

    /// <summary>
    /// Determines whether the Install command can execute.
    /// </summary>
    /// <returns>
    /// <c>true</c> if an install folder is selected and the user has agreed; otherwise, <c>false</c>.
    /// </returns>
    private bool CanInstall()
    {
        return !string.IsNullOrEmpty(InstallFolder) && Agree;
    }

    #endregion

    #region Relay Commands

    /// <summary>
    /// Opens the license URL in the default browser.
    /// </summary>
    [RelayCommand]
    private void License()
    {
        ShellHelper.LaunchUrl(_bootstrapperApplicationManager.LicenseUrl);
    }

    /// <summary>
    /// Opens a folder browser dialog for the user to select an installation folder.
    /// </summary>
    [RelayCommand]
    private void BrowseInstallFolder()
    {
        using var folderDialog = new FolderBrowserDialog
        {
            Description = Resources.Select_installation_folder,
            SelectedPath = InstallFolder,
            ShowNewFolderButton = true
        };

        if (folderDialog.ShowDialog() == DialogResult.OK)
        {
            InstallFolder = folderDialog.SelectedPath;
        }
    }

    /// <summary>
    /// Executes the installation process with the selected options and navigates to the progress page.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanInstall))]
    private async Task Install()
    {
        _bootstrapperApplicationManager.InstallDirectory = InstallFolder;
        _bootstrapperApplicationManager.CreateDesktopShortcut = CreateDesktopShortcut;
        _bootstrapperApplicationManager.CreateStartMenuShortcut = CreateStartMenuShortcut;
        _bootstrapperApplicationManager.LaunchOnStartup = LaunchOnStartup;
        await _navigationService.NavigateToAsync(PageKeys.InstallProgressPage);
    }

    /// <summary>
    /// Cancels the installation and closes the application.
    /// </summary>
    [RelayCommand]
    private void Cancel()
    {
        _bootstrapperApplicationManager.Close();
    }

    #endregion
}
