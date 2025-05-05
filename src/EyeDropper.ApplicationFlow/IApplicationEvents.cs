namespace EyeDropper.ApplicationFlow;

/// <summary>
/// Defines a centralized application event system for raising and handling domain events.
/// </summary>
public interface IApplicationEvents
{
    /// <summary>
    /// Raises a domain event asynchronously via the configured synchronization context.
    /// </summary>
    /// <typeparam name="T">The type of the domain event.</typeparam>
    /// <param name="domainEvent">The event instance to raise.</param>
    void Raise<T>(T domainEvent) where T : ApplicationEvent;

    /// <summary>
    /// Registers a delegate to handle events of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of event to subscribe to.</typeparam>
    /// <param name="eventHandler">The handler delegate invoked when the event is raised.</param>
    void Register<T>(Action<T> eventHandler) where T : ApplicationEvent;

    /// <summary>
    /// Unregisters a previously registered delegate for the specified event type.
    /// </summary>
    /// <typeparam name="T">The type of event to unsubscribe from.</typeparam>
    /// <param name="eventHandler">The handler delegate to remove.</param>
    void UnRegister<T>(Action<T> eventHandler) where T : ApplicationEvent;

    /// <summary>
    /// Unregisters all delegates whose target matches the provided object instance.
    /// </summary>
    /// <typeparam name="T">The type of event handlers to remove.</typeparam>
    /// <param name="obj">The instance whose handlers should be removed.</param>
    void UnRegister<T>(object obj);
}
