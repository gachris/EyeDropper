using CommonServiceLocator;
using EyeDropper.Bootstrapper.Core.Contracts;
using EyeDropper.Bootstrapper.Host.WixToolset;
using EyeDropper.Bootstrapper.UI;
using EyeDropper.Bootstrapper.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using ApplicationHost = Microsoft.Extensions.Hosting.Host;

namespace EyeDropper.Bootstrapper.Host;

/// <summary>
/// Configures the dependency injection container and application host
/// for the EyeDropper bootstrapper application.
/// </summary>
public static class IocConfiguration
{
    #region Properties

    /// <summary>
    /// Gets the configured <see cref="IHost"/> instance for the application,
    /// or <c>null</c> if <see cref="Setup"/> has not been called.
    /// </summary>
    public static IHost AppHost { get; private set; } = null!;

    #endregion

    #region Methods

    /// <summary>
    /// Builds and configures the <see cref="IHost"/> for the EyeDropper bootstrapper,
    /// including logging and service registrations, and sets up the service locator.
    /// </summary>
    /// <param name="bAManager">
    /// The bootstrapper application manager that implements <see cref="IWpfBootstrapperApplicationManager"/>.
    /// </param>
    public static void Setup()
    {
        AppHost = ApplicationHost.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddFilter<EventLogLoggerProvider>(level => false);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<ShellWindow>();
                services.AddSingleton(_ => ServiceLocator.Current);
                services.AddSingleton(WpfBootstrapperApplication.Current.BootstrapperApplicationManager);
                services.AddUI();
            })
            .Build();

        // Set up the global service locator for legacy code paths
        ServiceLocator.SetLocatorProvider(() => new AppServiceLocator(AppHost.Services));
    }

    #endregion
}
