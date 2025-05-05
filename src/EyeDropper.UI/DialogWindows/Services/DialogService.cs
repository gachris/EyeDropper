using System.Windows;
using EyeDropper.UI.DialogWindows.ViewModels;
using EyeDropper.UI.DialogWindows.Views;

namespace EyeDropper.UI.DialogWindows.Services;

/// <summary>
/// Provides functionality to show modal dialog windows and track their association with view models.
/// </summary>
public class DialogService : IDialogService
{
    #region Fields/Consts

    /// <summary>
    /// Holds the mapping between dialog view models and their corresponding dialog windows.
    /// </summary>
    private readonly Dictionary<DialogViewModel, DialogWindow> _dialogs = [];

    #endregion

    #region IDialogService Implementation

    /// <summary>
    /// Shows a modal dialog window for the specified view model.
    /// </summary>
    /// <param name="owner">
    /// The window that owns the dialog. If <c>null</c>, the application's main window
    /// is used as the owner if it is visible; otherwise, the dialog is centered on screen.
    /// </param>
    /// <param name="viewModel">The <see cref="DialogViewModel"/> to display in the dialog.</param>
    /// <param name="options">Options controlling the appearance and behavior of the dialog.</param>
    /// <returns>
    /// A <see cref="ModalResult"/> indicating how the dialog was closed (e.g., OK or Cancel).
    /// </returns>
    public ModalResult ShowDialog(Window? owner, DialogViewModel viewModel, DialogOptions options)
    {
        using var dialog = new DialogWindow(new DialogView(viewModel), options);

        _dialogs.Add(viewModel, dialog);

        dialog.Owner = owner
            ?? (System.Windows.Application.Current.MainWindow.IsVisible
                ? System.Windows.Application.Current.MainWindow
                : null);
        dialog.WindowStartupLocation = dialog.Owner is null
            ? WindowStartupLocation.CenterScreen
            : WindowStartupLocation.CenterOwner;

        var modalResult = dialog.ShowModal();

        _dialogs.Remove(viewModel);

        return modalResult;
    }

    /// <summary>
    /// Attempts to retrieve the dialog window associated with the specified view model.
    /// </summary>
    /// <param name="viewModel">
    /// The <see cref="DialogViewModel"/> whose dialog window is being requested.
    /// </param>
    /// <param name="dialogWindow">
    /// When this method returns, contains the <see cref="DialogWindow"/> associated with
    /// <paramref name="viewModel"/>, if found; otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if a dialog window was found for the specified view model; otherwise, <c>false</c>.
    /// </returns>
    public bool TryGetDialogByViewModel(DialogViewModel viewModel, out DialogWindow dialogWindow)
    {
        return _dialogs.TryGetValue(viewModel, out dialogWindow!);
    }

    #endregion
}
