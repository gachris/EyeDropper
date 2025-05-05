namespace EyeDropper.Bootstrapper.Core.Helpers;

/// <summary>
/// Utility class to work with HRESULTs
/// </summary>
public class HResult
{
    /// <summary>
    /// Determines if an HRESULT was a success code or not.
    /// </summary>
    /// <param name="status">HRESULT to verify.</param>
    /// <returns>True if the status is a success code.</returns>
    public static bool Succeeded(int status)
    {
        return status >= 0;
    }
}