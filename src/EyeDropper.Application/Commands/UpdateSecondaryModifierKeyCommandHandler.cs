using DevToolbox.Core.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateSecondaryModifierKeyCommand"/>, updating the secondary modifier key.
/// </summary>
public class UpdateSecondaryModifierKeyCommandHandler : UpdateSettingHandlerBase<UpdateSecondaryModifierKeyCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSecondaryModifierKeyCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateSecondaryModifierKeyCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateSecondaryModifierKeyCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateSecondaryModifierKeyCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateSecondaryModifierKey(request.SecondaryModifierKey);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.SecondaryModifierKey), RuntimeInfo.SecondaryModifierKey).ConfigureAwait(false);
    }
}
