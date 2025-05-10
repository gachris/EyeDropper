using CommonServiceLocator;
using DevToolbox.Core;
using DevToolbox.Core.Contracts;
using DevToolbox.Core.Services;
using EyeDropper.Application;
using EyeDropper.Core;
using EyeDropper.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EyeDropper.Host;

/// <summary>
/// Configures and initializes the dependency injection container and application host for EyeDropper.
/// </summary>
public static class IocConfiguration
{
    #region Properties

    /// <summary>
    /// Gets the configured <see cref="IHost"/> instance for the application.
    /// </summary>
    /// <value>
    /// The application host, built with registered services. 
    /// This will be <c>null</c> until <see cref="Setup"/> is called.
    /// </value>
    public static IHost? AppHost { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Builds and configures the application host, registers core, application, and UI services,
    /// and sets up the service locator for resolving dependencies.
    /// </summary>
    public static void Setup()
    {
        AppHost = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((context, services) =>
            {
                var localSettingsOptionsConfiguration = context.Configuration.GetSection(nameof(LocalSettingsOptions));

                if (SynchronizationContext.Current is not null)
                {
                    services.AddSingleton(SynchronizationContext.Current);
                }

                services.Configure<LocalSettingsOptions>(localSettingsOptionsConfiguration);
                services.AddSingleton<IApplication>(sp => (App)System.Windows.Application.Current);
                services.AddSingleton(_ => ServiceLocator.Current);
                services.AddCore();
                services.AddApplication();
                services.AddUI();
            })
            .Build();

        ServiceLocator.SetLocatorProvider(() => new AppServiceLocator(AppHost.Services));
    }

    #endregion
}