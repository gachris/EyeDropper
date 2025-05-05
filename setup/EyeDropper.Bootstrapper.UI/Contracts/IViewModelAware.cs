namespace EyeDropper.Bootstrapper.UI.Contracts;

/// <summary>
/// Provides methods for view models to respond to navigation events.
/// </summary>
public interface IViewModelAware
{
    /// <summary>
    /// Gets a value indicating whether the current view can navigate back to the previous view.
    /// This is typically used to determine the availability of a "Back" command or navigation control.
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    /// Called when the view model's associated view has been navigated to.
    /// </summary>
    void OnNavigated();

    /// <summary>
    /// Called when the view model's associated view is being navigated away from.
    /// </summary>
    void OnNavigatedAway();
}
