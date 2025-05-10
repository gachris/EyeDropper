using DevToolbox.Core.Contracts;
using EyeDropper.Application.Constants;
using EyeDropper.Application.Dtos;
using DevToolbox.Core.ApplicationFlow;
using EyeDropper.Core.RuntimeOptions;
using EyeDropper.Core.RuntimeOptions.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handles <see cref="LoadRuntimeOptionsCommand"/> by loading settings from local storage,
/// applying them to the runtime options state, and raising corresponding domain events.
/// </summary>
public class LoadRuntimeSettingsCommandHandler : IRequestHandler<LoadRuntimeOptionsCommand, RequestResultDto>
{
    #region Fields/Consts

    private readonly IRuntimeOptionsState _runtimeOptionsState;
    private readonly ILocalSettingsService _localSettingsService;
    private readonly IApplicationEvents _applicationEvents;
    private readonly ILogger<LoadRuntimeSettingsCommandHandler> _logger;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadRuntimeSettingsCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeOptionsState">The state object to update with loaded settings.</param>
    /// <param name="localSettingsService">Service for reading and writing local settings.</param>
    /// <param name="applicationEvents">Event system for raising lifecycle events.</param>
    /// <param name="logger">Logger for recording errors and diagnostics.</param>
    public LoadRuntimeSettingsCommandHandler(
        IRuntimeOptionsState runtimeOptionsState,
        ILocalSettingsService localSettingsService,
        IApplicationEvents applicationEvents,
        ILogger<LoadRuntimeSettingsCommandHandler> logger)
    {
        _runtimeOptionsState = runtimeOptionsState;
        _localSettingsService = localSettingsService;
        _applicationEvents = applicationEvents;
        _logger = logger;
    }

    #region Methods

    /// <summary>
    /// Processes the <see cref="LoadRuntimeOptionsCommand"/>, loading each setting and
    /// updating the <see cref="IRuntimeOptionsState"/> accordingly.
    /// </summary>
    /// <param name="request">The command instance (ignored in this implementation).</param>
    /// <param name="cancellationToken">Token to observe for cancellation.</param>
    /// <returns>
    /// A <see cref="RequestResultDto"/> indicating success or failure and any errors.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// Thrown if the operation is canceled before completion.
    /// </exception>
    public async Task<RequestResultDto> Handle(LoadRuntimeOptionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _applicationEvents.Raise(new LoadRuntimeOptionsStartedEvent());

            var copyToClipboard = await _localSettingsService
                .ReadSettingAsync<bool?>(nameof(IRuntimeOptionsState.CopyToClipboard))
                .ConfigureAwait(false)
                ?? true;
            _runtimeOptionsState.UpdateCopyToClipboard(copyToClipboard);

            var primaryModifierKey = await _localSettingsService
                .ReadSettingAsync<int?>(nameof(IRuntimeOptionsState.PrimaryModifierKey))
                .ConfigureAwait(false)
                ?? default;
            _runtimeOptionsState.UpdatePrimaryModifierKey(primaryModifierKey);

            var secondaryModifierKey = await _localSettingsService
                .ReadSettingAsync<int?>(nameof(IRuntimeOptionsState.SecondaryModifierKey))
                .ConfigureAwait(false)
                ?? default;
            _runtimeOptionsState.UpdateSecondaryModifierKey(secondaryModifierKey);

            var tertiaryModifierKey = await _localSettingsService
                .ReadSettingAsync<int?>(nameof(IRuntimeOptionsState.TertiaryModifierKey))
                .ConfigureAwait(false)
                ?? 1;
            _runtimeOptionsState.UpdateTertiaryModifierKey(tertiaryModifierKey);

            var hotKey = await _localSettingsService
                .ReadSettingAsync<int?>(nameof(IRuntimeOptionsState.HotKey))
                .ConfigureAwait(false)
                ?? 146;
            _runtimeOptionsState.UpdateHotKey(hotKey);

            var colorFormat = await _localSettingsService
                .ReadSettingAsync<int?>(nameof(IRuntimeOptionsState.ColorFormat))
                .ConfigureAwait(false)
                ?? default;
            _runtimeOptionsState.UpdateColorFormat(colorFormat);

            var precision = await _localSettingsService
                .ReadSettingAsync<int?>(nameof(IRuntimeOptionsState.Precision))
                .ConfigureAwait(false)
                ?? 1;
            _runtimeOptionsState.UpdatePrecision(precision);

            var htmlTemplate = await _localSettingsService
                .ReadSettingAsync<string?>(nameof(IRuntimeOptionsState.HTMLTemplate))
                .ConfigureAwait(false)
                ?? ColorTemplates.HTML.NumberSignRGB;
            _runtimeOptionsState.UpdateHTMLTemplate(htmlTemplate);

            var hexTemplate = await _localSettingsService
                .ReadSettingAsync<string?>(nameof(IRuntimeOptionsState.HexTemplate))
                .ConfigureAwait(false)
                ?? ColorTemplates.Hex.BGR;
            _runtimeOptionsState.UpdateHexTemplate(hexTemplate);

            var delphiHexTemplate = await _localSettingsService
                .ReadSettingAsync<string?>(nameof(IRuntimeOptionsState.DelphiHexTemplate))
                .ConfigureAwait(false)
                ?? ColorTemplates.DelphiHex.BGR;
            _runtimeOptionsState.UpdateDelphiHexTemplate(delphiHexTemplate);

            var vbHexTemplate = await _localSettingsService
                .ReadSettingAsync<string?>(nameof(IRuntimeOptionsState.VBHexTemplate))
                .ConfigureAwait(false)
                ?? ColorTemplates.VBHex.BGR;
            _runtimeOptionsState.UpdateVBHexTemplate(vbHexTemplate);

            var rgbTemplate = await _localSettingsService
                .ReadSettingAsync<string?>(nameof(IRuntimeOptionsState.RGBTemplate))
                .ConfigureAwait(false)
                ?? ColorTemplates.RGB.CommaSeparated;
            _runtimeOptionsState.UpdateRGBTemplate(rgbTemplate);

            var rgbFloatTemplate = await _localSettingsService
                .ReadSettingAsync<string?>(nameof(IRuntimeOptionsState.RGBFloatTemplate))
                .ConfigureAwait(false)
                ?? ColorTemplates.RGBFloat.CommaSeparated;
            _runtimeOptionsState.UpdateRGBFloatTemplate(rgbFloatTemplate);

            var hsvTemplate = await _localSettingsService
                .ReadSettingAsync<string?>(nameof(IRuntimeOptionsState.HSVTemplate))
                .ConfigureAwait(false)
                ?? ColorTemplates.HSV.CommaSeparated;
            _runtimeOptionsState.UpdateHSVTemplate(hsvTemplate);

            var hslTemplate = await _localSettingsService
                .ReadSettingAsync<string?>(nameof(IRuntimeOptionsState.HSLTemplate))
                .ConfigureAwait(false)
                ?? ColorTemplates.HSL.CommaSeparated;
            _runtimeOptionsState.UpdateHSLTemplate(hslTemplate);

            var longTemplate = await _localSettingsService
                .ReadSettingAsync<string?>(nameof(IRuntimeOptionsState.LongTemplate))
                .ConfigureAwait(false)
                ?? ColorTemplates.Long.Number;
            _runtimeOptionsState.UpdateLongTemplate(longTemplate);

            return new RequestResultDto();
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Read runtime settings operation canceled.");

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while reading runtime settings.");

            return new RequestResultDto("Unable to read runtime settings.", ex);
        }
        finally
        {
            _applicationEvents.Raise(new LoadRuntimeOptionsFinishedEvent());
        }
    }

    #endregion
}