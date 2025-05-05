namespace EyeDropper.Bootstrapper.Core;

/// <summary>
/// Base class for strongly-typed Wix variables, providing a name and abstract methods
/// for getting and setting the variable value via the Bootstrapper engine.
/// </summary>
/// <typeparam name="T">The type of the variable (e.g., <see cref="bool"/> or <see cref="string"/>).</typeparam>
public abstract class WixVariable<T>
{
    #region Properties

    /// <summary>
    /// Gets the name of the Wix variable as defined in the bundle manifest.
    /// </summary>
    public string WixVariableName { get; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="WixVariable{T}"/> class with the specified variable name.
    /// </summary>
    /// <param name="wixVariableName">The name of the Wix variable to wrap.</param>
    protected WixVariable(string wixVariableName)
    {
        WixVariableName = wixVariableName;
    }

    #region Methods

    /// <summary>
    /// Retrieves the current value of the variable from the Bootstrapper engine.
    /// </summary>
    /// <returns>The current value of type <typeparamref name="T"/>.</returns>
    public abstract T Get();

    /// <summary>
    /// Updates the variable in the Bootstrapper engine to the specified value.
    /// </summary>
    /// <param name="value">The new value to set.</param>
    /// <returns><c>true</c> if the engine variable was changed; otherwise, <c>false</c>.</returns>
    public abstract bool Set(T value);

    #endregion
}
