using System.Windows;
using CommonServiceLocator;
using DevToolbox.Wpf;
using DevToolbox.Wpf.Media;

namespace EyeDropper.Bootstrapper.Host;

/// <summary>
/// Interaction logic for the WPF application entry point for the EyeDropper bootstrapper host.
/// Manages the application lifecycle and enforces a single-instance of the installer.
/// </summary>
public partial class App : Application
{
    #region Fields/Consts

    /// <summary>
    /// The event mutex name used for inter-process signaling between instances.
    /// </summary>
    private const string UniqueEventName = "0b9092d8-c940-47ec-9b32-18b9d758b863";

    /// <summary>
    /// The mutex name used to ensure only one instance of the installer runs.
    /// </summary>
    private const string UniqueMutexName = "EyeDropper.Setup";

    /// <summary>
    /// Manages single-instance enforcement and focus requests.
    /// </summary>
    private readonly SingletonApplicationManager _singletonApplicationManager;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    /// <param name="bAManager">The EyeDropper bootstrapper application manager.</param>
    public App()
    {
        _singletonApplicationManager = new SingletonApplicationManager(UniqueEventName, UniqueMutexName);

        InitializeComponent();
    }

    #region Methods

    /// <summary>
    /// Called when the application is starting up. Sets the theme, enforces single-instance,
    /// and initializes the dependency injection container if this is the first instance.
    /// </summary>
    /// <param name="e">Startup event arguments, including command-line parameters.</param>
    protected override async void OnStartup(StartupEventArgs e)
    {
        ThemeManager.RequestedTheme = ElementTheme.WindowsDefault;

        _singletonApplicationManager.Register(
            this,
            onRegistered: IocConfiguration.Setup,
            onInstanceAlreadyRunning: () =>
            {
                MessageBox.Show(
                    Localization.Properties.Resources.Another_instance_of_the_EyeDropper_setup_is_already_running,
                    Localization.Properties.Resources.EyeDropper_Installer,
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            });

        await IocConfiguration.AppHost.StartAsync().ConfigureAwait(false);

        var shell = ServiceLocator.Current.GetInstance<ShellWindow>();
        MainWindow = shell;

        shell.Show();

        base.OnStartup(e);
    }

    #endregion
}
