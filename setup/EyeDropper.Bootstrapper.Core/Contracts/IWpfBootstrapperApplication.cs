using WixToolset.BootstrapperApplicationApi;

namespace EyeDropper.Bootstrapper.Core.Contracts;

/// <summary>
/// Defines the contract for the bootstrapper application,
/// extending the default WiX bootstrapper application functionality.
/// </summary>
public interface IWpfBootstrapperApplication : IDefaultBootstrapperApplication
{
    /// <summary>
    /// Gets the WiX engine instance used for logging
    /// and interacting with the installation process.
    /// </summary>
    IEngine Engine { get; }

    /// <summary>
    /// Gets the bootstrapper command data provided at application startup,
    /// including any command-line arguments.
    /// </summary>
    IBootstrapperCommand Command { get; }

    /// <summary>
    /// Gets the manifest data for the bootstrapper application,
    /// containing package definitions and payload metadata.
    /// </summary>
    IBootstrapperApplicationData Manifest { get; }
}