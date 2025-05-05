namespace EyeDropper.ApplicationFlow;

/// <summary>
/// Dispatches domain events from an aggregate root to the application event system.
/// </summary>
public class ApplicationEventsDispatcher : IApplicationEventsDispatcher
{
    #region Fields/Consts

    private readonly IApplicationEvents _applicationEvents;
    private readonly IAggregateRoot _aggregateRoot;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationEventsDispatcher"/> class.
    /// </summary>
    /// <param name="aggregateRoot">The aggregate root to collect events from.</param>
    /// <param name="applicationEvents">The application events service to raise events to.</param>
    public ApplicationEventsDispatcher(IAggregateRoot aggregateRoot, IApplicationEvents applicationEvents)
    {
        _aggregateRoot = aggregateRoot;
        _applicationEvents = applicationEvents;
    }

    #region IEventsDispatcher Implementation

    /// <summary>
    /// Retrieves events from the aggregate root in sequence order and raises them via <see cref="IApplicationEvents"/>.
    /// </summary>
    public void Dispatch()
    {
        var events = _aggregateRoot.TakeApplicationEvents()
            .OrderBy(ev => ev.Index)
            .ToList()
            .AsReadOnly();

        foreach (var e in events)
        {
            _applicationEvents.Raise(e);
        }
    }

    #endregion
}