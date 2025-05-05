using System.Runtime.InteropServices;

namespace EyeDropper.Host.Interop;

/// <summary>
/// Provides access to selected functions in the Windows User32 API.
/// </summary>
internal static class User32
{
    /// <summary>
    /// Brings the thread that created the specified window into the foreground and activates the window.
    /// </summary>
    /// <param name="hWnd">
    /// A handle to the window that should be activated and brought to the foreground.
    /// </param>
    /// <returns>
    /// <c>true</c> if the window was brought to the foreground; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This function will fail if the calling process does not have permission or if the system restricts
    /// which processes can set the foreground window. For more information, see the Win32 documentation
    /// for SetForegroundWindow.
    /// </remarks>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);
}
