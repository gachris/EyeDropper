using System.Windows;
using CommonServiceLocator;
using EyeDropper.Localization.Properties;
using EyeDropper.UI.DialogWindows;
using EyeDropper.UI.DialogWindows.Services;
using EyeDropper.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace EyeDropper.Host.Helpers;

/// <summary>
/// Handles global exceptions for the application by subscribing to AppDomain, Dispatcher, and TaskScheduler events
/// and displaying error dialogs when unhandled exceptions occur.
/// </summary>
public static class GlobalExceptionHandler
{
    #region Fields/Consts

    /// <summary>
    /// Default dialog options for showing unhandled exception information.
    /// </summary>
    private static readonly DialogOptions ErrorDialogOptions = new()
    {
        Width = 458,
        MinHeight = 185,
        MaxHeight = 560,
        SizeToContent = SizeToContent.Height,
        WindowTitle = Resources.Unhandled_exception,
        PluginButtons =
        [
            new()
            {
                IsDefault = true,
                ButtonOrder = 10,
                ButtonPosition = PluginButtonPosition.Right,
                ButtonType = PluginButtonType.OK,
            }
        ]
    };

    #endregion

    #region Methods

    /// <summary>
    /// Subscribes to global exception events (AppDomain, Dispatcher, and TaskScheduler)
    /// to ensure all unhandled exceptions are caught and displayed to the user.
    /// </summary>
    public static void SetupExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            ShowErrorDialog((Exception)e.ExceptionObject);

        System.Windows.Application.Current.DispatcherUnhandledException += (s, e) =>
        {
            ShowErrorDialog(e.Exception);
            e.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            ShowErrorDialog(e.Exception);
            e.SetObserved();
        };
    }

    /// <summary>
    /// Retrieves the dialog service from the service locator and uses it to show an error dialog
    /// containing the exception message and stack trace.
    /// </summary>
    /// <param name="exception">The exception to display.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <see cref="IDialogService"/> cannot be resolved from the service locator.
    /// </exception>
    private static void ShowErrorDialog(Exception exception)
    {
        var dialogService = ServiceLocator.Current.GetService<IDialogService>();

        ArgumentNullException.ThrowIfNull(dialogService, nameof(dialogService));

        dialogService.ShowDialog(
            owner: null,
            viewModel: new ErrorDialogViewModel(exception.Message, exception.StackTrace),
            options: ErrorDialogOptions
        );
    }

    #endregion
}