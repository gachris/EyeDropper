using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateColorFormatCommand"/>, updating the color format setting.
/// </summary>
public class UpdateColorFormatCommandHandler : UpdateSettingHandlerBase<UpdateColorFormatCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateColorFormatCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateColorFormatCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateColorFormatCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateColorFormatCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateColorFormat(request.ColorFormat);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.ColorFormat), RuntimeInfo.ColorFormat).ConfigureAwait(false);
    }
}
