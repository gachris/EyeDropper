using CommunityToolkit.Mvvm.ComponentModel;
using EyeDropper.UI.DialogWindows.Views;

namespace EyeDropper.UI.DialogWindows.ViewModels;

/// <summary>
/// Base view model for dialog windows, providing lifecycle hooks and disposal.
/// </summary>
public abstract partial class DialogViewModel : ObservableObject, IDisposable
{
    #region Fields/Consts

    private DialogWindow _hostWindow = default!;
    private bool _disposedValue;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the <see cref="DialogWindow"/> hosting this view model.
    /// Setting this property invokes <see cref="AfterLoad"/> once assigned.
    /// </summary>
    public DialogWindow HostWindow
    {
        get => _hostWindow;
        set
        {
            _hostWindow = value;
            AfterLoad();
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Called after the view model has been loaded into its host window.
    /// Override in derived classes to perform initialization logic.
    /// </summary>
    protected virtual void AfterLoad()
    {
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Releases resources used by the <see cref="DialogViewModel"/>.
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
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer if needed
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    /// <summary>
    /// Releases all resources used by the <see cref="DialogViewModel"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // /// <summary>
    // /// Finalizer to clean up unmanaged resources.
    // /// </summary>
    // ~DialogViewModel()
    // {
    //     Dispose(disposing: false);
    // }

    #endregion
}
