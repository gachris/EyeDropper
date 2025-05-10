using DevToolbox.Core.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateRGBFloatTemplateCommand"/>, updating the floating-point RGB template.
/// </summary>
public class UpdateRGBFloatTemplateCommandHandler : UpdateSettingHandlerBase<UpdateRGBFloatTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRGBFloatTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateRGBFloatTemplateCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateRGBFloatTemplateCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateRGBFloatTemplateCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateRGBFloatTemplate(request.RGBFloatTemplate);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.RGBFloatTemplate), RuntimeInfo.RGBFloatTemplate).ConfigureAwait(false);
    }
}
