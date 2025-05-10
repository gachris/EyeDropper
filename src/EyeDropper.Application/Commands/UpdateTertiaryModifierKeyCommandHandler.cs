using DevToolbox.Core.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateTertiaryModifierKeyCommand"/>, updating the tertiary modifier key.
/// </summary>
public class UpdateTertiaryModifierKeyCommandHandler : UpdateSettingHandlerBase<UpdateTertiaryModifierKeyCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTertiaryModifierKeyCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateTertiaryModifierKeyCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateTertiaryModifierKeyCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateTertiaryModifierKeyCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateTertiaryModifierKey(request.TertiaryModifierKey);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.TertiaryModifierKey), RuntimeInfo.TertiaryModifierKey).ConfigureAwait(false);
    }
}
