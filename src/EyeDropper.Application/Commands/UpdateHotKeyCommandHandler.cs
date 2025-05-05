using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateHotKeyCommand"/>, updating the hotkey setting.
/// </summary>
public class UpdateHotKeyCommandHandler : UpdateSettingHandlerBase<UpdateHotKeyCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateHotKeyCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateHotKeyCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateHotKeyCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateHotKeyCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateHotKey(request.HotKey);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.HotKey), RuntimeInfo.HotKey).ConfigureAwait(false);
    }
}
