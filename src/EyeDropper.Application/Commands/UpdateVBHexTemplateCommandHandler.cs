using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateVBHexTemplateCommand"/>, updating the VB hex template.
/// </summary>
public class UpdateVBHexTemplateCommandHandler : UpdateSettingHandlerBase<UpdateVBHexTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateVBHexTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateVBHexTemplateCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateVBHexTemplateCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateVBHexTemplateCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateVBHexTemplate(request.VBHexTemplate);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.VBHexTemplate), RuntimeInfo.VBHexTemplate).ConfigureAwait(false);
    }
}
