using DevToolbox.Core.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateHSVTemplateCommand"/>, updating the HSV template.
/// </summary>
public class UpdateHSVTemplateCommandHandler : UpdateSettingHandlerBase<UpdateHSVTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateHSVTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateHSVTemplateCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateHSVTemplateCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateHSVTemplateCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateHSVTemplate(request.HSVTemplate);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.HSVTemplate), RuntimeInfo.HSVTemplate).ConfigureAwait(false);
    }
}
