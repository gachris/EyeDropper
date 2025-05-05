using CommonServiceLocator;

namespace EyeDropper.ApplicationFlow;

/// <summary>
/// Provides a centralized event dispatcher for raising and handling domain events within the application.
/// Combines in-memory delegate handlers with IoC-resolved <see cref="IApplicationEventHandler{T}"/> implementations.
/// </summary>
public class ApplicationEvents : IApplicationEvents
{
    #region Fields/Consts

    private readonly IServiceLocator _serviceLocator;
    private readonly SynchronizationContext _context;
    private readonly Dictionary<Type, List<Delegate>> _handlers = [];

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationEvents"/> class.
    /// </summary>
    /// <param name="serviceLocator">Service locator for resolving event handler implementations.</param>
    /// <param name="context">Synchronization context for dispatching events on the correct thread.</param>
    public ApplicationEvents(IServiceLocator serviceLocator, SynchronizationContext context)
    {
        _serviceLocator = serviceLocator;
        _context = context;
    }

    #region IApplicationEvents Implementation

    /// <summary>
    /// Raises a domain event asynchronously, posting to the synchronization context.
    /// </summary>
    /// <typeparam name="T">The type of the event being raised.</typeparam>
    /// <param name="domainEvent">The event instance to raise.</param>
    public void Raise<T>(T domainEvent) where T : ApplicationEvent
    {
        _context.Post(state => RaiseInternal(domainEvent), null);
    }

    /// <summary>
    /// Registers an in-memory delegate to handle events of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The event type to subscribe to.</typeparam>
    /// <param name="eventHandler">The delegate to invoke when the event is raised.</param>
    public void Register<T>(Action<T> eventHandler) where T : ApplicationEvent
    {
        var type = typeof(T);

        if (!_handlers.TryGetValue(type, out var value))
        {
            value = [];
            _handlers[type] = value;
        }

        value.Add(eventHandler);
    }

    /// <summary>
    /// Unregisters a previously registered delegate for events of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The event type to unsubscribe from.</typeparam>
    /// <param name="eventHandler">The delegate instance to remove.</param>
    public void UnRegister<T>(Action<T> eventHandler) where T : ApplicationEvent
    {
        var type = typeof(T);
        if (_handlers.TryGetValue(type, out var value) && value.Contains(eventHandler))
        {
            value.Remove(eventHandler);

            if (_handlers[type].Count == 0)
            {
                _handlers.Remove(type);
            }
        }
    }

    /// <summary>
    /// Unregisters all delegates whose target matches the specified object instance.
    /// </summary>
    /// <typeparam name="T">The event type to unsubscribe from.</typeparam>
    /// <param name="obj">The target object whose handlers should be removed.</param>
    public void UnRegister<T>(object obj)
    {
        foreach (var handler in _handlers.ToList())
        {
            foreach (var v in handler.Value.ToList().Where(v => v.Target == obj))
            {
                handler.Value.Remove(v);
            }
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Internal method to invoke both in-memory delegates and IoC-resolved <see cref="IApplicationEventHandler{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The event type being processed.</typeparam>
    /// <param name="domainEvent">The event instance to dispatch.</param>
    private void RaiseInternal<T>(T domainEvent) where T : ApplicationEvent
    {
        var domainEventType = domainEvent.GetType();

        if (_handlers.TryGetValue(domainEventType, out var value))
        {
            foreach (var handler in value)
            {
                handler.DynamicInvoke(domainEvent);
            }
        }

        var type = typeof(IApplicationEventHandler<>).MakeGenericType(domainEventType);
        var handlers = _serviceLocator.GetAllInstances(type).ToList<dynamic?>();
        handlers.ForEach(e =>
        {
            e?.Handle((dynamic)domainEvent);
        });
    }

    #endregion
}
