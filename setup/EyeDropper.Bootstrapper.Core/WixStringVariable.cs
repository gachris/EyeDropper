using WixToolset.BootstrapperApplicationApi;

namespace EyeDropper.Bootstrapper.Core;

/// <summary>
/// Represents a String Wix variable that can be retrieved from and set on the Bootstrapper engine.
/// </summary>
public class WixStringVariable : WixVariable<string>
{
    #region Fields/Consts

    private readonly IEngine _engine;
    private bool _isValueRetrieved;
    private string _value = null!;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="WixStringVariable"/> class with the specified engine and variable name.
    /// </summary>
    /// <param name="engine">The Bootstrapper engine used to get and set variables.</param>
    /// <param name="wixVariableName">The name of the Wix variable.</param>
    public WixStringVariable(IEngine engine, string wixVariableName) : base(wixVariableName)
    {
        _engine = engine;
    }

    #region Methods Overrides

    /// <summary>
    /// Gets the current string value of the Wix variable, retrieving it from the engine on first access.
    /// </summary>
    /// <returns>The string value of the variable.</returns>
    public override string Get()
    {
        if (!_isValueRetrieved)
        {
            // Retrieve the variable's value from the engine
            _value = _engine.GetVariableString(WixVariableName);
            _isValueRetrieved = true;
        }

        return _value;
    }

    /// <summary>
    /// Sets the string value of the Wix variable on the engine if the new value differs from the current one.
    /// </summary>
    /// <param name="value">The new string value to set.</param>
    /// <returns><c>true</c> if the value was changed; otherwise, <c>false</c>.</returns>
    public override bool Set(string value)
    {
        if (_value != value)
        {
            _value = value;
            // Update the engine with the new string value
            _engine.SetVariableString(WixVariableName, _value, false);
            return true;
        }
        return false;
    }

    #endregion
}
