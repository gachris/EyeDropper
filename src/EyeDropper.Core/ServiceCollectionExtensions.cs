using DevToolbox.Core.ApplicationFlow;
using EyeDropper.Core.Client;
using Microsoft.Extensions.DependencyInjection;

namespace EyeDropper.Core;

/// <summary>
/// Provides extension methods to register the core EyeDropper services with an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the core EyeDropper application services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to which the core services should be added.</param>
    /// <returns>The original <see cref="IServiceCollection"/>, allowing for chaining.</returns>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IAggregateRoot, ClientRoot>();
        services.AddSingleton<IClientRoot>(s => (ClientRoot)s.GetService<IAggregateRoot>()!);
        services.AddSingleton(s => s.GetService<IClientRoot>()!.RuntimeOptionsState);

        services.AddSingleton<IApplicationEvents, ApplicationEvents>();
        services.AddSingleton<IApplicationEventsDispatcher, ApplicationEventsDispatcher>();

        return services;
    }
}
