using System.ComponentModel;
using System.Windows;
using DevToolbox.Wpf.Controls;
using EyeDropper.UI.Contracts;
using EyeDropper.UI.ViewModels;

namespace EyeDropper.UI.Views;

/// <summary>
/// Hosts the eye dropper control in a window and implements <see cref="IEyeDropperService"/>.
/// </summary>
public partial class EyeDropperHost : Window, IEyeDropperService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EyeDropperHost"/> class with the specified view model.
    /// </summary>
    /// <param name="viewModel">The <see cref="EyeDropperViewModel"/> to bind to the view.</param>
    public EyeDropperHost(EyeDropperViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    #region IEyeDropperService

    /// <summary>
    /// Starts the color capture process. If the host is not visible, it will be shown; otherwise it will be activated.
    /// </summary>
    public void StartCapture()
    {
        Dispatcher.BeginInvoke(() =>
        {
            if (!IsVisible)
            {
                Show();
            }
            else
            {
                Activate();
            }

            EyeDropperControl.Capture();
        });
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Handles the window closing event by canceling the close and hiding the window instead.
    /// </summary>
    /// <param name="e">
    /// The <see cref="CancelEventArgs"/> containing event data.
    /// </param>
    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles the <see cref="CaptureChanged"/> event of the <see cref="EyeDropperControl"/>.
    /// Closes the window when capture is finished.
    /// </summary>
    private void EyeDropperControl_CaptureChanged(object? sender, CaptureEventArgs e)
    {
        if (e.CaptureState == CaptureState.Finished)
        {
            Close();
        }
    }

    #endregion
}
