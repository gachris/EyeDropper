namespace EyeDropper.Bootstrapper.Core;

/// <summary>
/// Defines the various states of the installation lifecycle.
/// </summary>
public enum InstallationState
{
    /// <summary>
    /// The bootstrapper is initializing.
    /// </summary>
    Initializing,

    /// <summary>
    /// The bootstrapper is detecting the presence or absence of the product.
    /// </summary>
    Detecting,

    /// <summary>
    /// The bootstrapper is waiting for user input or another event.
    /// </summary>
    Waiting,

    /// <summary>
    /// The bootstrapper is planning the actions to perform.
    /// </summary>
    Planning,

    /// <summary>
    /// The bootstrapper is applying the planned actions.
    /// </summary>
    Applying,

    /// <summary>
    /// The bootstrapper has successfully applied all actions.
    /// </summary>
    Applied,

    /// <summary>
    /// The bootstrapper encountered a failure during installation.
    /// </summary>
    Failed,
}