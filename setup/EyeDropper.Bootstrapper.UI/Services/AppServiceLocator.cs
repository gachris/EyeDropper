using CommonServiceLocator;
using Microsoft.Extensions.DependencyInjection;

namespace EyeDropper.Bootstrapper.UI.Services;

/// <summary>
/// Provides a service locator implementation that wraps an <see cref="IServiceProvider"/> instance,
/// enabling runtime access to registered services, including keyed services.
/// </summary>
public class AppServiceLocator : IServiceLocator
{
    #region Fields/Consts

    private readonly IServiceProvider _host;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="AppServiceLocator"/> class.
    /// </summary>
    /// <param name="host">The application-wide service provider.</param>
    public AppServiceLocator(IServiceProvider host)
    {
        _host = host;
    }

    #region IServiceLocator Implementation

    /// <summary>
    /// Gets all registered instances of the specified service type.
    /// </summary>
    public IEnumerable<object?> GetAllInstances(Type serviceType)
    {
        return _host.GetServices(serviceType);
    }

    /// <summary>
    /// Gets all registered instances of the specified generic service type.
    /// </summary>
    public IEnumerable<TService> GetAllInstances<TService>()
    {
        return _host.GetServices<TService>();
    }

    /// <summary>
    /// Gets a required instance of the specified service type.
    /// Throws an exception if the service is not registered.
    /// </summary>
    public object GetInstance(Type serviceType)
    {
        return _host.GetRequiredService(serviceType);
    }

    /// <summary>
    /// Gets a required instance of the specified keyed service type.
    /// Throws an exception if the service is not registered.
    /// </summary>
    public object GetInstance(Type serviceType, string key)
    {
        return _host.GetRequiredKeyedService(serviceType, key);
    }

    /// <summary>
    /// Gets a required instance of the specified generic service type.
    /// Throws an exception if the service is not registered.
    /// </summary>
    public TService GetInstance<TService>() where TService : notnull
    {
        return _host.GetRequiredService<TService>();
    }

    /// <summary>
    /// Gets a required instance of the specified generic keyed service type.
    /// Throws an exception if the service is not registered.
    /// </summary>
    public TService? GetInstance<TService>(string key) where TService : notnull
    {
        return _host.GetRequiredKeyedService<TService>(key);
    }

    /// <summary>
    /// Gets a service instance if it is registered, or null otherwise.
    /// </summary>
    public object? GetService(Type serviceType)
    {
        return _host.GetService(serviceType);
    }

    #endregion
}
