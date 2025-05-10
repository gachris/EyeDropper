using DevToolbox.Core.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdatePrimaryModifierKeyCommand"/>, updating the primary modifier key.
/// </summary>
public class UpdatePrimaryModifierKeyCommandHandler : UpdateSettingHandlerBase<UpdatePrimaryModifierKeyCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePrimaryModifierKeyCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdatePrimaryModifierKeyCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdatePrimaryModifierKeyCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdatePrimaryModifierKeyCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdatePrimaryModifierKey(request.PrimaryModifierKey);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.PrimaryModifierKey), RuntimeInfo.PrimaryModifierKey).ConfigureAwait(false);
    }
}
