namespace EyeDropper.ApplicationFlow;

/// <summary>
/// Defines the contract for an aggregate root in the domain model, enabling retrieval of domain events.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Retrieves and clears the list of uncommitted domain events from the aggregate.
    /// </summary>
    /// <returns>A read-only list of <see cref="ApplicationEvent"/> instances representing raised domain events.</returns>
    IReadOnlyList<ApplicationEvent> TakeApplicationEvents();
}
