namespace EyeDropper.ApplicationFlow;

/// <summary>
/// Base class for domain events, assigning each event a unique, monotonically increasing sequence index.
/// </summary>
public abstract class ApplicationEvent
{
    #region Fields/Consts

    private static long _index;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the unique sequence index of this event instance.
    /// </summary>
    public long Index { get; } = GetNextIndex();

    #endregion

    #region Methods

    /// <summary>
    /// Atomically increments the internal index counter and returns the new value.
    /// </summary>
    /// <returns>The next unique index value for an event.</returns>
    private static long GetNextIndex() => Interlocked.Increment(ref _index);

    #endregion
}
