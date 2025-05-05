using DevToolbox.Wpf.Windows;
using EyeDropper.UI.DialogWindows.ViewModels;

namespace EyeDropper.UI.DialogWindows.Views;

/// <summary>
/// Represents a customizable dialog window with support for plugin content, waiting animations,
/// and modal result handling.
/// </summary>
public partial class DialogWindow : WindowEx, IDisposable
{
    #region Fields/Consts

    private bool _disposedValue;
    private ModalResult _modalResult;
    private DialogWindowViewModel? _dialogWindowViewModel;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the result of the dialog once it closes.
    /// Setting this property closes the window and stops any waiting animation.
    /// </summary>
    public ModalResult ModalResult
    {
        get => _modalResult;
        set
        {
            DialogResult = true;
            _modalResult = value;
            StopWaitingAnimation();
            Close();
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new, empty instance of the <see cref="DialogWindow"/> class.
    /// </summary>
    public DialogWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogWindow"/> class,
    /// injecting the specified view and dialog options.
    /// </summary>
    /// <param name="view">The <see cref="DialogView"/> to host inside this window.</param>
    /// <param name="dialogOptions">Options controlling size, style, and animations.</param>
    public DialogWindow(DialogView view, DialogOptions dialogOptions)
        : this()
    {
        InitializeViewComponent(view, dialogOptions);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Configures the window based on the provided view and options,
    /// wiring up the view model and window properties.
    /// </summary>
    /// <param name="view">The view to display inside the dialog.</param>
    /// <param name="dialogOptions">The options to apply to this window.</param>
    private void InitializeViewComponent(DialogView view, DialogOptions dialogOptions)
    {
        view.AttachToWindowEvents(this);

        Width = dialogOptions.Width;
        Height = dialogOptions.Height;
        MaxWidth = dialogOptions.MaxWidth;
        MaxHeight = dialogOptions.MaxHeight;
        MinWidth = dialogOptions.MinWidth;
        MinHeight = dialogOptions.MinHeight;
        SizeToContent = dialogOptions.SizeToContent;
        ResizeMode = dialogOptions.ResizeMode;
        Title = dialogOptions.WindowTitle ?? string.Empty;
        IsTitleBarVisible = dialogOptions.IsTitleBarVisible;

        if (ResizeMode == System.Windows.ResizeMode.NoResize)
        {
            Chrome.ResizeBorderThickness = new System.Windows.Thickness(0);
        }

        if (!dialogOptions.IsTitleBarVisible)
        {
            Chrome.CaptionHeight = 0;
        }

        _dialogWindowViewModel = new DialogWindowViewModel(this, view, dialogOptions)
        {
            WaitingAnimationTitle = dialogOptions.AnimationTitle,
            WaitingAnimationMessage = dialogOptions.AnimationMessage
        };

        DataContext = _dialogWindowViewModel;
    }

    /// <summary>
    /// Starts the waiting animation displayed in the dialog footer or content.
    /// </summary>
    public void StartWaitingAnimation()
    {
        if (_dialogWindowViewModel is not null)
            _dialogWindowViewModel.WaitingAnimationBusy = true;
    }

    /// <summary>
    /// Stops the waiting animation if it is running.
    /// </summary>
    public void StopWaitingAnimation()
    {
        if (_dialogWindowViewModel is not null)
            _dialogWindowViewModel.WaitingAnimationBusy = false;
    }

    /// <summary>
    /// Displays the dialog as a modal window and returns its <see cref="ModalResult"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="ModalResult"/> indicating how the dialog was closed,
    /// or <see cref="ModalResult.Cancel"/> if the dialog was dismissed.
    /// </returns>
    public ModalResult ShowModal()
    {
        return ShowDialog() == true
            ? ModalResult
            : ModalResult.Cancel;
    }

    #endregion

    #region IDisposable Implementation

    /// <summary>
    /// Releases managed and unmanaged resources used by this instance.
    /// </summary>
    /// <param name="disposing">
    /// <c>true</c> to release both managed and unmanaged resources;
    /// <c>false</c> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _dialogWindowViewModel?.Dispose();
                _dialogWindowViewModel = null;
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
    /// Suppresses finalization.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // /// <summary>
    // /// Finalizer to release unmanaged resources if Dispose was not called.
    // /// </summary>
    // ~DialogWindow()
    // {
    //     Dispose(disposing: false);
    // }

    #endregion
}
