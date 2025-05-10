using System.Windows;
using System.Windows.Interop;
using CommonServiceLocator;
using DevToolbox.Core.Contracts;
using DevToolbox.Wpf;
using EyeDropper.Host.Interop;
using EyeDropper.UI.Contracts;
using Hardcodet.Wpf.TaskbarNotification;

namespace EyeDropper.Host;

/// <summary>
/// WPF application entry point for EyeDropper, enforcing single-instance behavior,
/// configuring dependency injection, global exception handling, and tray icon interactions.
/// </summary>
public partial class App : System.Windows.Application, IApplication
{
    #region Fields/Consts

    /// <summary>
    /// The taskbar key.
    /// </summary>
    private const string TaskbarIconKey = "Taskbar";

    /// <summary>
    /// The event mutex name.
    /// </summary>
    private const string UniqueEventName = "D00F493F-D831-49BA-9198-D8B1ED8237AA";

    /// <summary>
    /// The unique mutex name.
    /// </summary>
    private const string UniqueMutexName = "EyeDropper";

    /// <summary>
    /// The singleton application manager.
    /// </summary>
    private readonly SingletonApplicationManager _singletonApplicationManager;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class,
    /// setting up the singleton application manager.
    /// </summary>
    public App()
    {
        _singletonApplicationManager = new SingletonApplicationManager(UniqueEventName, UniqueMutexName);
    }

    #region Methods

    /// <summary>
    /// Called when the application starts. Registers singleton behavior,
    /// sets up services, configures global exception handling, initializes components,
    /// and shows a tray balloon tip.
    /// </summary>
    /// <param name="e">The startup event arguments.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _singletonApplicationManager.Register(this,
            onRegistered: async () =>
            {
                IocConfiguration.Setup();
                GlobalExceptionHandler.SetupExceptionHandling();

                // Get Instance to initialize xaml components
                ServiceLocator.Current.GetInstance<IEyeDropperService>();

                var appUISettings = ServiceLocator.Current.GetInstance<IAppUISettings>();
                await appUISettings.InitializeAsync();

                var taskbarIcon = (TaskbarIcon)FindResource(TaskbarIconKey);
                taskbarIcon.TrayBalloonTipClicked += Taskbar_TrayBalloonTipClicked;

                taskbarIcon.ShowBalloonTip(null, Localization.Properties.Resources.Application_running_message, BalloonIcon.Info);
            },
            onInstanceAlreadyRunning: () =>
            {
                var taskbarIcon = (TaskbarIcon)FindResource(TaskbarIconKey);
                taskbarIcon.ShowBalloonTip(null, Localization.Properties.Resources.Application_already_running_message, BalloonIcon.Info);
            });
    }

    #endregion

    #region Events Subscriptions

    /// <summary>
    /// Handles clicks on the tray balloon tip by opening the context menu
    /// and bringing the window to the foreground.
    /// </summary>
    /// <param name="sender">The <see cref="TaskbarIcon"/> that raised the event.</param>
    /// <param name="e">The routed event arguments.</param>
    private static void Taskbar_TrayBalloonTipClicked(object sender, RoutedEventArgs e)
    {
        var taskbarIcon = (TaskbarIcon)sender;
        taskbarIcon.ContextMenu.IsOpen = true;

        var source = (HwndSource)PresentationSource.FromVisual(taskbarIcon.ContextMenu);
        if (source != null)
        {
            User32.SetForegroundWindow(source.Handle);
        }
    }

    #endregion
}