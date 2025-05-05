namespace EyeDropper.ApplicationFlow;

/// <summary>
/// Base class for aggregate roots in the domain model, providing event tracking and child aggregation support.
/// </summary>
public abstract class AggregateRoot : IAggregateRoot
{
    #region Fields/Consts

    /// <summary>
    /// Internal list of domain events raised by this aggregate.
    /// </summary>
    private readonly List<ApplicationEvent> _applicationEvents = [];

    /// <summary>
    /// Synchronization object to ensure thread-safe operations on events.
    /// </summary>
    protected readonly object Locker = new();

    #endregion

    #region Properties

    /// <summary>
    /// Gets the child aggregates of this aggregate root.
    /// Override to include nested AggregateRoot instances.
    /// </summary>
    protected virtual IEnumerable<AggregateRoot> ChildAggregates => [];

    #endregion

    #region Methods

    /// <summary>
    /// Clears all tracked events on this aggregate and its children.
    /// </summary>
    public void ClearEvents()
    {
        lock (Locker)
        {
            _applicationEvents.Clear();
            foreach (var child in ChildAggregates)
            {
                child.ClearEvents();
            }
        }
    }

    /// <summary>
    /// Retrieves and clears all events raised by this aggregate and its children.
    /// </summary>
    /// <returns>A read-only list of <see cref="ApplicationEvent"/> instances representing raised events.</returns>
    public virtual IReadOnlyList<ApplicationEvent> TakeApplicationEvents()
    {
        lock (Locker)
        {
            var events = _applicationEvents.ToList();
            _applicationEvents.Clear();
            foreach (var child in ChildAggregates)
            {
                events.AddRange(child.TakeApplicationEvents());
            }
            return events.AsReadOnly();
        }
    }

    /// <summary>
    /// Records a domain event for this aggregate.
    /// </summary>
    /// <param name="e">The <see cref="ApplicationEvent"/> to add to the event list.</param>
    protected void AddEvent(ApplicationEvent e)
    {
        lock (Locker)
        {
            _applicationEvents.Add(e);
        }
    }

    #endregion
}
