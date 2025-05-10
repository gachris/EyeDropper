using DevToolbox.Core.Contracts;
using DevToolbox.Core.Services;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application;

/// <summary>
/// Provides extension methods to register application-layer services and MediatR behaviors.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds application services, MediatR handlers, and logging to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to which application services will be added.</param>
    /// <returns>The original <see cref="IServiceCollection"/>, allowing for chained method calls.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ILocalSettingsService, LocalSettingsService>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
        services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(RequestPostProcessor<,>));

        services.AddLogging(loggingBuilder =>
        {
            services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
        });

        return services;
    }
}