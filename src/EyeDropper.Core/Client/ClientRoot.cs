using DevToolbox.Core.ApplicationFlow;
using EyeDropper.Core.Client.Events;
using EyeDropper.Core.RuntimeOptions;

namespace EyeDropper.Core.Client;

/// <summary>
/// Represents the root aggregate for the client, holding child aggregates such as runtime options state.
/// Raises an event when the runtime options state instance is set.
/// </summary>
public class ClientRoot : AggregateRoot, IClientRoot
{
    #region Fields/Consts

    /// <summary>
    /// Backing field for the <see cref="RuntimeOptionsState"/> property.
    /// </summary>
    private IRuntimeOptionsState _runtimeOptionsState = default!;

    #endregion

    #region Properties

    /// <inheritdoc/>
    protected override IEnumerable<AggregateRoot> ChildAggregates =>
        new[] { (AggregateRoot)RuntimeOptionsState };

    /// <summary>
    /// Gets the runtime options state aggregate.
    /// When set, clears the state and raises a <see cref="RuntimeOptionsStateSetEvent"/>.
    /// </summary>
    public IRuntimeOptionsState RuntimeOptionsState
    {
        get => _runtimeOptionsState;
        private set
        {
            _runtimeOptionsState = value;
            _runtimeOptionsState.ClearEvents();
            AddEvent(new RuntimeOptionsStateSetEvent());
        }
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientRoot"/> class
    /// and sets up the default <see cref="RuntimeOptionsState"/>.
    /// </summary>
    public ClientRoot()
    {
        RuntimeOptionsState = new RuntimeOptionsState();
    }
}
