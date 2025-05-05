using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateRGBTemplateCommand"/>, updating the RGB template.
/// </summary>
public class UpdateRGBTemplateCommandHandler : UpdateSettingHandlerBase<UpdateRGBTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRGBTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateRGBTemplateCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateRGBTemplateCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateRGBTemplateCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateRGBTemplate(request.RGBTemplate);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.RGBTemplate), RuntimeInfo.RGBTemplate).ConfigureAwait(false);
    }
}
