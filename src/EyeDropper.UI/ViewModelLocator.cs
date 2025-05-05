using CommonServiceLocator;
using EyeDropper.UI.ViewModels;

namespace EyeDropper.UI;

/// <summary>
/// Provides access to shared view model instances via the service locator.
/// </summary>
public class ViewModelLocator
{
    /// <summary>
    /// Gets the singleton <see cref="TaskbarViewModel"/> instance from the service locator.
    /// </summary>
    public static TaskbarViewModel TaskbarViewModel
        => ServiceLocator.Current.GetInstance<TaskbarViewModel>();

    /// <summary>
    /// Gets the singleton <see cref="AdvanceColorPickerDialogViewModel"/> instance from the service locator.
    /// </summary>
    public static AdvanceColorPickerDialogViewModel AdvanceColorPickerDialogViewModel
        => ServiceLocator.Current.GetInstance<AdvanceColorPickerDialogViewModel>();

    /// <summary>
    /// Gets the singleton <see cref="SettingsViewModel"/> instance from the service locator.
    /// </summary>
    public static SettingsViewModel SettingsViewModel
        => ServiceLocator.Current.GetInstance<SettingsViewModel>();
}
