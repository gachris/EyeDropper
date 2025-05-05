using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateHexTemplateCommand"/>, updating the hex template.
/// </summary>
public class UpdateHexTemplateCommandHandler : UpdateSettingHandlerBase<UpdateHexTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateHexTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateHexTemplateCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateHexTemplateCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateHexTemplateCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateHexTemplate(request.HexTemplate);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.HexTemplate), RuntimeInfo.HexTemplate).ConfigureAwait(false);
    }
}
