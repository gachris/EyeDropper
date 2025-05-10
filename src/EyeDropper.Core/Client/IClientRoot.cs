using DevToolbox.Core.ApplicationFlow;
using EyeDropper.Core.RuntimeOptions;

namespace EyeDropper.Core.Client;

/// <summary>
/// Defines the root aggregate for the client, exposing child aggregates such as the runtime options state.
/// </summary>
public interface IClientRoot : IAggregateRoot
{
    /// <summary>
    /// Gets the runtime options state aggregate.
    /// </summary>
    IRuntimeOptionsState RuntimeOptionsState { get; }
}
