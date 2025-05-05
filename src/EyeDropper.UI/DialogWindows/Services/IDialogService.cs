using System.Windows;
using EyeDropper.UI.DialogWindows.ViewModels;
using EyeDropper.UI.DialogWindows.Views;

namespace EyeDropper.UI.DialogWindows.Services;

/// <summary>
/// Defines methods for displaying and managing modal dialog windows.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Displays a modal dialog for the specified view model.
    /// </summary>
    /// <param name="owner">
    /// The window that will own the dialog. If <c>null</c>, the main application window
    /// is used if visible; otherwise, the dialog is centered on screen.
    /// </param>
    /// <param name="viewModel">The view model to host in the dialog.</param>
    /// <param name="options">Options controlling dialog appearance and behavior.</param>
    /// <returns>
    /// A <see cref="ModalResult"/> indicating how the user closed the dialog (e.g., OK or Cancel).
    /// </returns>
    ModalResult ShowDialog(Window? owner, DialogViewModel viewModel, DialogOptions options);

    /// <summary>
    /// Attempts to retrieve an open dialog window associated with the given view model.
    /// </summary>
    /// <param name="viewModel">The view model whose dialog window is sought.</param>
    /// <param name="dialogWindow">
    /// When this method returns, contains the associated <see cref="DialogWindow"/> if found; otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if a matching dialog window was found; otherwise, <c>false</c>.
    /// </returns>
    bool TryGetDialogByViewModel(DialogViewModel viewModel, out DialogWindow dialogWindow);
}
