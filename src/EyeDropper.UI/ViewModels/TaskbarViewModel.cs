using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevToolbox.Wpf.Controls;
using EyeDropper.Application.Commands;
using EyeDropper.ApplicationFlow;
using EyeDropper.Core.RuntimeOptions;
using EyeDropper.Core.RuntimeOptions.Events;
using EyeDropper.Localization.Properties;
using EyeDropper.UI.Contracts;
using EyeDropper.UI.DialogWindows;
using EyeDropper.UI.DialogWindows.Services;
using MediatR;
using NHotkey;
using NHotkey.Wpf;

namespace EyeDropper.UI.ViewModels;

/// <summary>
/// ViewModel for the taskbar control, handling hotkey registration, commands, and dialog interactions.
/// </summary>
public partial class TaskbarViewModel : ObservableObject
{
    #region Fields/Consts

    private const string CaptureColorHotKeyName = "CaptureColor";

    private static readonly DialogOptions AdvanceColorPickerDialogOptions = new()
    {
        Width = 620,
        Height = 420,
        MinHeight = 420,
        MinWidth = 620,
        SizeToContent = SizeToContent.WidthAndHeight,
        ResizeMode = ResizeMode.NoResize,
        WindowTitle = Resources.Advance_Color_Picker
    };

    private static readonly DialogOptions SettingsDialogOptions = new()
    {
        Width = 1000,
        Height = 700,
        MinHeight = 336,
        MinWidth = 520,
        ResizeMode = ResizeMode.CanResize,
        WindowTitle = Resources.Eye_dropper
    };

    private readonly IRuntimeOptionsState _runtimeInfo;
    private readonly IMediator _mediator;
    private readonly IApplication _application;
    private readonly IDialogService _dialogService;
    private readonly IEyeDropperService _eyeDropperHostService;

    private ColorFormat _colorFormat;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the currently selected <see cref="ColorFormat"/> (reflects runtime setting).
    /// </summary>
    public ColorFormat ColorFormat
    {
        get => _colorFormat;
        private set => SetProperty(ref _colorFormat, value);
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskbarViewModel"/> class,
    /// registering for runtime option updates and starting initialization.
    /// </summary>
    /// <param name="runtimeInfo">Provides access to current runtime options.</param>
    /// <param name="mediator">Mediates application commands.</param>
    /// <param name="application">Provides application-level operations (e.g., shutdown).</param>
    /// <param name="dialogService">Manages dialog windows.</param>
    /// <param name="applicationEvents">Publishes runtime option change events.</param>
    /// <param name="eyeDropperHostService">Service to start the eye dropper capture.</param>
    public TaskbarViewModel(
        IRuntimeOptionsState runtimeInfo,
        IMediator mediator,
        IApplication application,
        IDialogService dialogService,
        IApplicationEvents applicationEvents,
        IEyeDropperService eyeDropperHostService)
    {
        _runtimeInfo = runtimeInfo;
        _mediator = mediator;
        _application = application;
        _dialogService = dialogService;
        _eyeDropperHostService = eyeDropperHostService;

        applicationEvents.Register<ColorFormatUpdatedEvent>((e) => ColorFormat = (ColorFormat)_runtimeInfo.ColorFormat);
        applicationEvents.Register<HotKeyUpdatedEvent>((e) => UpdateHotKeySet());
        applicationEvents.Register<PrimaryModifierKeyUpdatedEvent>((e) => UpdateHotKeySet());
        applicationEvents.Register<SecondaryModifierKeyUpdatedEvent>((e) => UpdateHotKeySet());
        applicationEvents.Register<TertiaryModifierKeyUpdatedEvent>((e) => UpdateHotKeySet());

        ColorFormat = (ColorFormat)_runtimeInfo.ColorFormat;

        Initialize();
    }

    #region Methods

    /// <summary>
    /// Sends the command to load runtime options, then sets up the hotkey.
    /// </summary>
    private async void Initialize()
    {
        await _mediator.Send(new LoadRuntimeOptionsCommand());
        UpdateHotKeySet();
    }

    /// <summary>
    /// Registers or replaces the global hotkey used to trigger color capture.
    /// </summary>
    private void UpdateHotKeySet()
    {
        HotkeyManager.Current.AddOrReplace(
            CaptureColorHotKeyName,
            (Key)_runtimeInfo.HotKey,
            (ModifierKeys)_runtimeInfo.PrimaryModifierKey | (ModifierKeys)_runtimeInfo.SecondaryModifierKey | (ModifierKeys)_runtimeInfo.TertiaryModifierKey,
            OnHotkeyPressed);
    }

    #endregion
    
    #region Relay Commands

    /// <summary>
    /// Command to start the eye dropper capture process.
    /// </summary>
    [RelayCommand]
    private void Capture()
    {
        _eyeDropperHostService.StartCapture();
    }

    /// <summary>
    /// Command to update the application's color format setting.
    /// </summary>
    /// <param name="colorFormat">The new <see cref="ColorFormat"/> to apply.</param>
    [RelayCommand]
    private async Task SetColorFormat(ColorFormat colorFormat)
    {
        await _mediator.Send(new UpdateColorFormatCommand((int)colorFormat));
    }

    /// <summary>
    /// Command to open or activate the advanced color picker dialog.
    /// </summary>
    [RelayCommand]
    private void OpenAdvanceColorPicker()
    {
        if (_dialogService.TryGetDialogByViewModel(
                ViewModelLocator.AdvanceColorPickerDialogViewModel,
                out var dialogWindow))
        {
            dialogWindow.Activate();
        }
        else
        {
            _dialogService.ShowDialog(
                null,
                ViewModelLocator.AdvanceColorPickerDialogViewModel,
                AdvanceColorPickerDialogOptions);
        }
    }

    /// <summary>
    /// Command to open or activate the settings dialog.
    /// </summary>
    [RelayCommand]
    private void OpenSettings()
    {
        if (_dialogService.TryGetDialogByViewModel(
                ViewModelLocator.SettingsViewModel,
                out var dialogWindow))
        {
            dialogWindow.Activate();
        }
        else
        {
            _dialogService.ShowDialog(
                null,
                ViewModelLocator.SettingsViewModel,
                SettingsDialogOptions);
        }
    }

    /// <summary>
    /// Command to exit the application gracefully.
    /// </summary>
    [RelayCommand]
    private void ExitApplication()
    {
        _application.Shutdown();
    }

    #endregion

    #region Events Subscriptions

    /// <summary>
    /// Handles the global hotkey press event by initiating capture.
    /// </summary>
    private void OnHotkeyPressed(object? sender, HotkeyEventArgs e)
    {
        _eyeDropperHostService.StartCapture();
        e.Handled = true;
    }

    #endregion
}