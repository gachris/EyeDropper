using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateHSLTemplateCommand"/>, updating the HSL template.
/// </summary>
public class UpdateHSLTemplateCommandHandler : UpdateSettingHandlerBase<UpdateHSLTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateHSLTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateHSLTemplateCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateHSLTemplateCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateHSLTemplateCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateHSLTemplate(request.HSLTemplate);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.HSLTemplate), RuntimeInfo.HSLTemplate).ConfigureAwait(false);
    }
}
