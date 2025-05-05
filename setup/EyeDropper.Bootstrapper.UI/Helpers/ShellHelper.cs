using System.Diagnostics;

namespace EyeDropper.Bootstrapper.UI.Helpers;

/// <summary>
/// Provides helper methods for launching URIs or opening folders
/// using the system’s default shell (e.g., default browser, file explorer).
/// </summary>
internal static class ShellHelper
{
    /// <summary>
    /// Launches the default web browser to the provided URI.
    /// </summary>
    /// <param name="uri">URI to open the web browser.</param>
    public static void LaunchUrl(string uri)
    {
        UseShellExecute(uri);
    }

    /// <summary>
    /// Launches the default web browser to the provided URI.
    /// </summary>
    /// <param name="uri">URI to open the web browser.</param>
    internal static void LaunchUrl(Uri uri)
    {
        UseShellExecute(uri.ToString());
    }

    /// <summary>
    /// Open a log folder.
    /// </summary>
    /// <param name="uri">path to a log folder.</param>
    private static void UseShellExecute(string path)
    {
        var cursor = System.Windows.Application.Current.MainWindow.Cursor;
        System.Windows.Application.Current.MainWindow.Cursor = System.Windows.Input.Cursors.Wait;
        Process? process = null;
        try
        {
            process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Verb = "open";

            process.Start();
        }
        finally
        {
            process?.Dispose();
            System.Windows.Application.Current.MainWindow.Cursor = cursor;
        }
    }
}