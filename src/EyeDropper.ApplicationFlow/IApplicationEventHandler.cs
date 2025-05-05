namespace EyeDropper.ApplicationFlow;

/// <summary>
/// Defines a handler for processing domain events of a specific type.
/// </summary>
/// <typeparam name="T">The type of <see cref="ApplicationEvent"/> this handler processes.</typeparam>
public interface IApplicationEventHandler<in T> where T : ApplicationEvent
{
    /// <summary>
    /// Handles the specified domain event.
    /// </summary>
    /// <param name="e">The event instance to handle.</param>
    void Handle(T e);
}