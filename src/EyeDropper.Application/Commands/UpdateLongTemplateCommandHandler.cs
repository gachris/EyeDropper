using DevToolbox.Core.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateLongTemplateCommand"/>, updating the comprehensive long template.
/// </summary>
public class UpdateLongTemplateCommandHandler : UpdateSettingHandlerBase<UpdateLongTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLongTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateLongTemplateCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateLongTemplateCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateLongTemplateCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateLongTemplate(request.LongTemplate);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.LongTemplate), RuntimeInfo.LongTemplate).ConfigureAwait(false);
    }
}