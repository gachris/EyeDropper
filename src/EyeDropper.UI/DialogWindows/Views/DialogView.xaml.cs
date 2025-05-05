using System.Windows.Controls;
using EyeDropper.UI.DialogWindows.ViewModels;

namespace EyeDropper.UI.DialogWindows.Views;

/// <summary>
/// Represents the view for a dialog, hosting the content provided by a <see cref="DialogViewModel"/>.
/// </summary>
public partial class DialogView : UserControl, IDisposable
{
    #region Fields/Consts

    private DialogViewModel? _dialogViewModel;
    private bool _disposedValue;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the dialog window that hosts this view.
    /// </summary>
    protected DialogWindow? HostWindow { get; private set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogView"/> class and sets its data context.
    /// </summary>
    /// <param name="pluginViewModel">
    /// The <see cref="DialogViewModel"/> providing the content and logic for this view.
    /// </param>
    public DialogView(DialogViewModel pluginViewModel)
    {
        InitializeComponent();

        DataContext = pluginViewModel;
        _dialogViewModel = pluginViewModel;
    }

    #region Methods

    /// <summary>
    /// Attaches this view to the specified <see cref="DialogWindow"/>'s events and assigns the host window
    /// to the underlying view model.
    /// </summary>
    /// <param name="dialogWindow">The <see cref="DialogWindow"/> that will host this view.</param>
    public void AttachToWindowEvents(DialogWindow dialogWindow)
    {
        if (_dialogViewModel == null) return;

        HostWindow = dialogWindow;
        _dialogViewModel.HostWindow = HostWindow;
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Releases the managed resources used by the <see cref="DialogView"/>.
    /// </summary>
    /// <param name="disposing">
    /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _dialogViewModel?.Dispose();
                _dialogViewModel = null;
                HostWindow = null;
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the <see cref="DialogView"/> and suppresses finalization.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // /// <summary>
    // /// Finalizer to ensure unmanaged resources are released if Dispose was not called.
    // /// </summary>
    // ~DialogView()
    // {
    //     Dispose(disposing: false);
    // }

    #endregion
}
