using System.IO;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevToolbox.Core.Contracts;
using DevToolbox.Core.Media;
using DevToolbox.Wpf.Controls;
using DevToolbox.Wpf.Media;
using DevToolbox.Wpf.Windows.ViewModels;
using EyeDropper.Application.Commands;
using DevToolbox.Core.ApplicationFlow;
using EyeDropper.Core.RuntimeOptions;
using EyeDropper.Core.RuntimeOptions.Events;
using MediatR;
using Microsoft.Win32;

namespace EyeDropper.UI.ViewModels;

/// <summary>
/// ViewModel for the settings dialog, exposing application settings and responding to their changes.
/// </summary>
public partial class SettingsViewModel : DialogViewModel
{
    #region Fields/Consts

    private const string ApplicationName = "EyeDropper";
    private const string RunKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    private readonly IRuntimeOptionsState _runtimeOptionsState;
    private readonly IMediator _mediator;
    private readonly IAppUISettings _appUISettings;

    private Theme _theme;

    /// <summary>
    /// Gets or sets a value indicating whether the application will launch on Windows startup.
    /// </summary>
    [ObservableProperty]
    private bool _launchOnStartup;

    /// <summary>
    /// Gets or sets a value indicating whether captured colors are automatically copied to the clipboard.
    /// </summary>
    [ObservableProperty]
    private bool _copyToClipboard;

    /// <summary>
    /// Gets or sets the primary modifier key used in the global hotkey combination.
    /// </summary>
    [ObservableProperty]
    private ModifierKeys _primaryModifierKey;

    /// <summary>
    /// Gets or sets the secondary modifier key used in the global hotkey combination.
    /// </summary>
    [ObservableProperty]
    private ModifierKeys _secondaryModifierKey;

    /// <summary>
    /// Gets or sets the tertiary modifier key used in the global hotkey combination.
    /// </summary>
    [ObservableProperty]
    private ModifierKeys _tertiaryModifierKey;

    /// <summary>
    /// Gets or sets the main key used in the global hotkey combination for capture.
    /// </summary>
    [ObservableProperty]
    private Key _hotKey;

    /// <summary>
    /// Gets or sets the selected <see cref="ColorFormat"/> that determines how colors are formatted.
    /// </summary>
    [ObservableProperty]
    private ColorFormat _colorFormat;

    /// <summary>
    /// Gets or sets the number of decimal places to use when formatting numeric color components.
    /// </summary>
    [ObservableProperty]
    private int _precision;

    /// <summary>
    /// Gets or sets the HTML template string used for color formatting.
    /// </summary>
    [ObservableProperty]
    private string _hTMLTemplate = default!;

    /// <summary>
    /// Gets or sets the hexadecimal template string used for color formatting.
    /// </summary>
    [ObservableProperty]
    private string _hexTemplate = default!;

    /// <summary>
    /// Gets or sets the Delphi-style hexadecimal template string used for color formatting.
    /// </summary>
    [ObservableProperty]
    private string _delphiHexTemplate = default!;

    /// <summary>
    /// Gets or sets the VB-style hexadecimal template string used for color formatting.
    /// </summary>
    [ObservableProperty]
    private string _vBHexTemplate = default!;

    /// <summary>
    /// Gets or sets the RGB template string used for color formatting.
    /// </summary>
    [ObservableProperty]
    private string _rGBTemplate = default!;

    /// <summary>
    /// Gets or sets the floating-point RGB template string used for color formatting.
    /// </summary>
    [ObservableProperty]
    private string _rGBFloatTemplate = default!;

    /// <summary>
    /// Gets or sets the HSV template string used for color formatting.
    /// </summary>
    [ObservableProperty]
    private string _hSVTemplate = default!;

    /// <summary>
    /// Gets or sets the HSL template string used for color formatting.
    /// </summary>
    [ObservableProperty]
    private string _hSLTemplate = default!;

    /// <summary>
    /// Gets or sets the long (named) format template string used for color formatting.
    /// </summary>
    [ObservableProperty]
    private string _longTemplate = default!;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the current application version.
    /// </summary>
    public string ApplicationVersion { get; }

    /// <summary>
    /// Gets the company name extracted from assembly attributes.
    /// </summary>
    public string CompanyName { get; }

    /// <summary>
    /// Gets the copyright text extracted from assembly attributes.
    /// </summary>
    public string Copyright { get; }

    /// <summary>
    /// Gets the current UI theme.
    /// </summary>
    public Theme Theme
    {
        get => _theme;
        private set => SetProperty(ref _theme, value, nameof(Theme));
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of <see cref="SettingsViewModel"/>, loading current settings
    /// and registering for updates.
    /// </summary>
    /// <param name="runtimeOptionsState">Provides access to runtime option values.</param>
    /// <param name="mediator">Handles sending update commands.</param>
    /// <param name="appUISettings">Manages application UI settings like theme.</param>
    /// <param name="applicationEvents">Publishes events when settings change externally.</param>
    public SettingsViewModel(
        IRuntimeOptionsState runtimeOptionsState,
        IMediator mediator,
        IAppUISettings appUISettings,
        IApplicationEvents applicationEvents)
    {
        _runtimeOptionsState = runtimeOptionsState;
        _mediator = mediator;
        _appUISettings = appUISettings;

        ThemeManager.RequestedThemeChanged += ThemeManager_RequestedThemeChanged;

        var asm = Assembly.GetEntryAssembly();

        ApplicationVersion = asm?
            .GetName()?
            .Version?
            .ToString()
            ?? string.Empty;

        CompanyName = asm?
            .GetCustomAttribute<AssemblyCompanyAttribute>()?
            .Company
            ?? string.Empty;

        Copyright = asm?
            .GetCustomAttribute<AssemblyCopyrightAttribute>()?
            .Copyright
            ?? string.Empty;

        _theme = _appUISettings.Theme;
        _launchOnStartup = GetLaunchOnStartup();
        _copyToClipboard = _runtimeOptionsState.CopyToClipboard;
        _primaryModifierKey = (ModifierKeys)_runtimeOptionsState.PrimaryModifierKey;
        _secondaryModifierKey = (ModifierKeys)_runtimeOptionsState.SecondaryModifierKey;
        _tertiaryModifierKey = (ModifierKeys)_runtimeOptionsState.TertiaryModifierKey;
        _hotKey = (Key)_runtimeOptionsState.HotKey;
        _colorFormat = (ColorFormat)_runtimeOptionsState.ColorFormat;
        _precision = _runtimeOptionsState.Precision;
        _hTMLTemplate = _runtimeOptionsState.HTMLTemplate;
        _hexTemplate = _runtimeOptionsState.HexTemplate;
        _delphiHexTemplate = _runtimeOptionsState.DelphiHexTemplate;
        _vBHexTemplate = _runtimeOptionsState.VBHexTemplate;
        _rGBTemplate = _runtimeOptionsState.RGBTemplate;
        _rGBFloatTemplate = _runtimeOptionsState.RGBFloatTemplate;
        _hSVTemplate = _runtimeOptionsState.HSVTemplate;
        _hSLTemplate = _runtimeOptionsState.HSLTemplate;
        _longTemplate = _runtimeOptionsState.LongTemplate;

        applicationEvents.Register<CopyToClipboardUpdatedEvent>((e) => CopyToClipboard = _runtimeOptionsState.CopyToClipboard);
        applicationEvents.Register<PrimaryModifierKeyUpdatedEvent>((e) => PrimaryModifierKey = (ModifierKeys)_runtimeOptionsState.PrimaryModifierKey);
        applicationEvents.Register<SecondaryModifierKeyUpdatedEvent>((e) => SecondaryModifierKey = (ModifierKeys)_runtimeOptionsState.SecondaryModifierKey);
        applicationEvents.Register<TertiaryModifierKeyUpdatedEvent>((e) => TertiaryModifierKey = (ModifierKeys)_runtimeOptionsState.TertiaryModifierKey);
        applicationEvents.Register<HotKeyUpdatedEvent>((e) => HotKey = (Key)_runtimeOptionsState.HotKey);
        applicationEvents.Register<ColorFormatUpdatedEvent>((e) => ColorFormat = (ColorFormat)_runtimeOptionsState.ColorFormat);
        applicationEvents.Register<PrecisionUpdatedEvent>((e) => Precision = _runtimeOptionsState.Precision);
        applicationEvents.Register<HTMLTemplateUpdatedEvent>((e) => HTMLTemplate = _runtimeOptionsState.HTMLTemplate);
        applicationEvents.Register<HexTemplateUpdatedEvent>((e) => HexTemplate = _runtimeOptionsState.HexTemplate);
        applicationEvents.Register<DelphiHexTemplateUpdatedEvent>((e) => DelphiHexTemplate = _runtimeOptionsState.DelphiHexTemplate);
        applicationEvents.Register<VBHexTemplateUpdatedEvent>((e) => VBHexTemplate = _runtimeOptionsState.VBHexTemplate);
        applicationEvents.Register<RGBTemplateUpdatedEvent>((e) => RGBTemplate = _runtimeOptionsState.RGBTemplate);
        applicationEvents.Register<RGBFloatTemplateUpdatedEvent>((e) => RGBFloatTemplate = _runtimeOptionsState.RGBFloatTemplate);
        applicationEvents.Register<HSVTemplateUpdatedEvent>((e) => HSVTemplate = _runtimeOptionsState.HSVTemplate);
        applicationEvents.Register<HSLTemplateUpdatedEvent>((e) => HSLTemplate = _runtimeOptionsState.HSLTemplate);
        applicationEvents.Register<LongTemplateUpdatedEvent>((e) => LongTemplate = _runtimeOptionsState.LongTemplate);
    }

    #region Methods

    /// <summary>
    /// Determines whether the application is set to launch on Windows startup.
    /// </summary>
    /// <returns><c>true</c> if a registry startup entry exists; otherwise, <c>false</c>.</returns>
    private static bool GetLaunchOnStartup()
    {
        using var key = Registry.CurrentUser.OpenSubKey(RunKey, writable: false);
        return key?.GetValue(ApplicationName) != null;
    }

    #endregion

    #region Partial Property Change Handlers

    /// <summary>
    /// Called when the <see cref="LaunchOnStartup"/> property changes.
    /// Adds or removes the application entry in the registry.
    /// </summary>
    /// <param name="value">The new launch-on-startup value.</param>
    partial void OnLaunchOnStartupChanged(bool value)
    {
        using var key = Registry.CurrentUser.OpenSubKey(RunKey, writable: true);
        if (key == null) return;

        if (value)
        {
            var path = Path.Combine(Environment.CurrentDirectory, $"{ApplicationName}.exe");
            key.SetValue(ApplicationName, path);
        }
        else
        {
            key.DeleteValue(ApplicationName, throwOnMissingValue: false);
        }
    }

    /// <summary>
    /// Called when the <see cref="CopyToClipboard"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new copy-to-clipboard value.</param>
    partial void OnCopyToClipboardChanged(bool value)
        => _mediator.Send(new UpdateCopyToClipboardCommand(value));

    /// <summary>
    /// Called when the <see cref="PrimaryModifierKey"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new primary modifier key.</param>
    partial void OnPrimaryModifierKeyChanged(ModifierKeys value)
        => _mediator.Send(new UpdatePrimaryModifierKeyCommand((int)value));

    /// <summary>
    /// Called when the <see cref="SecondaryModifierKey"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new secondary modifier key.</param>
    partial void OnSecondaryModifierKeyChanged(ModifierKeys value)
        => _mediator.Send(new UpdateSecondaryModifierKeyCommand((int)value));

    /// <summary>
    /// Called when the <see cref="TertiaryModifierKey"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new tertiary modifier key.</param>
    partial void OnTertiaryModifierKeyChanged(ModifierKeys value)
        => _mediator.Send(new UpdateTertiaryModifierKeyCommand((int)value));

    /// <summary>
    /// Called when the <see cref="HotKey"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new hotkey.</param>
    partial void OnHotKeyChanged(Key value)
        => _mediator.Send(new UpdateHotKeyCommand((int)value));

    /// <summary>
    /// Called when the <see cref="ColorFormat"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new color format.</param>
    partial void OnColorFormatChanged(ColorFormat value)
        => _mediator.Send(new UpdateColorFormatCommand((int)value));

    /// <summary>
    /// Called when the <see cref="Precision"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new precision value.</param>
    partial void OnPrecisionChanged(int value)
        => _mediator.Send(new UpdatePrecisionCommand(value));

    /// <summary>
    /// Called when the <see cref="HTMLTemplate"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new HTML template string.</param>
    partial void OnHTMLTemplateChanged(string value)
        => _mediator.Send(new UpdateHTMLTemplateCommand(value));

    /// <summary>
    /// Called when the <see cref="HexTemplate"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new hex template string.</param>
    partial void OnHexTemplateChanged(string value)
        => _mediator.Send(new UpdateHexTemplateCommand(value));

    /// <summary>
    /// Called when the <see cref="DelphiHexTemplate"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new Delphi hex template string.</param>
    partial void OnDelphiHexTemplateChanged(string value)
        => _mediator.Send(new UpdateDelphiHexTemplateCommand(value));

    /// <summary>
    /// Called when the <see cref="VBHexTemplate"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new VB hex template string.</param>
    partial void OnVBHexTemplateChanged(string value)
        => _mediator.Send(new UpdateVBHexTemplateCommand(value));

    /// <summary>
    /// Called when the <see cref="RGBTemplate"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new RGB template string.</param>
    partial void OnRGBTemplateChanged(string value)
        => _mediator.Send(new UpdateRGBTemplateCommand(value));

    /// <summary>
    /// Called when the <see cref="RGBFloatTemplate"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new RGB float template string.</param>
    partial void OnRGBFloatTemplateChanged(string value)
        => _mediator.Send(new UpdateRGBFloatTemplateCommand(value));

    /// <summary>
    /// Called when the <see cref="HSVTemplate"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new HSV template string.</param>
    partial void OnHSVTemplateChanged(string value)
        => _mediator.Send(new UpdateHSVTemplateCommand(value));

    /// <summary>
    /// Called when the <see cref="HSLTemplate"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new HSL template string.</param>
    partial void OnHSLTemplateChanged(string value)
        => _mediator.Send(new UpdateHSLTemplateCommand(value));

    /// <summary>
    /// Called when the <see cref="LongTemplate"/> property changes.
    /// Persists the change via mediator command.
    /// </summary>
    /// <param name="value">The new long format template string.</param>
    partial void OnLongTemplateChanged(string value)
        => _mediator.Send(new UpdateLongTemplateCommand(value));

    #endregion

    #region Relay Commands

    /// <summary>
    /// Changes the application theme when the user selects a new one.
    /// </summary>
    /// <param name="theme">The new <see cref="ElementTheme"/> to apply.</param>
    [RelayCommand]
    private async Task ChangeTheme(Theme theme)
    {
        await _appUISettings.SetThemeAsync(theme);
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles theme change events from the <see cref="ThemeManager"/>.
    /// Updates the <see cref="Theme"/> property accordingly.
    /// </summary>
    private void ThemeManager_RequestedThemeChanged(object? sender, EventArgs e)
    {
        Theme = _appUISettings.Theme;
    }

    #endregion
}