using WixToolset.BootstrapperApplicationApi;

namespace EyeDropper.Bootstrapper.Core;

/// <summary>
/// Represents a Boolean Wix variable that can be retrieved from and set on the Bootstrapper engine.
/// </summary>
public class WixBooleanVariable : WixVariable<bool>
{
    #region Fields/Consts

    private readonly IEngine _engine;
    private bool _isValueRetrieved;
    private bool _value;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="WixBooleanVariable"/> class with the specified engine and variable name.
    /// </summary>
    /// <param name="engine">The Bootstrapper engine used to get and set variables.</param>
    /// <param name="wixVariableName">The name of the Wix variable.</param>
    public WixBooleanVariable(IEngine engine, string wixVariableName) : base(wixVariableName)
    {
        _engine = engine;
    }

    #region Methods Overrides

    /// <summary>
    /// Gets the current Boolean value of the Wix variable, retrieving it from the engine on first access.
    /// </summary>
    /// <returns>The Boolean value of the variable.</returns>
    public override bool Get()
    {
        if (!_isValueRetrieved)
        {
            // Wix stores boolean values as "1" for true and "0" for false
            _value = _engine.GetVariableNumeric(WixVariableName) == 1;
            _isValueRetrieved = true;
        }

        return _value;
    }

    /// <summary>
    /// Sets the Boolean value of the Wix variable on the engine if the new value differs from the current value.
    /// </summary>
    /// <param name="value">The new Boolean value to set.</param>
    /// <returns><c>true</c> if the value was changed; otherwise, <c>false</c>.</returns>
    public override bool Set(bool value)
    {
        if (_value != value)
        {
            _value = value;

            // Convert the Boolean to the "1"/"0" string representation that Wix expects
            _engine.SetVariableNumeric(WixVariableName, value ? 1 : 0);
            return true;
        }
        return false;
    }

    #endregion
}
