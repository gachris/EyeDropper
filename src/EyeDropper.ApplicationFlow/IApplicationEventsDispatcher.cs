namespace EyeDropper.ApplicationFlow;

/// <summary>
/// Defines a dispatcher responsible for triggering the processing of domain events
/// stored within the application event system.
/// </summary>
public interface IApplicationEventsDispatcher
{
    /// <summary>
    /// Initiates dispatch of all pending domain events through the configured
    /// <see cref="IApplicationEvents"/> implementation.
    /// </summary>
    void Dispatch();
}
