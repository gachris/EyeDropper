using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateDelphiHexTemplateCommand"/>, updating the Delphi hex template.
/// </summary>
public class UpdateDelphiHexTemplateCommandHandler : UpdateSettingHandlerBase<UpdateDelphiHexTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateDelphiHexTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateDelphiHexTemplateCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateDelphiHexTemplateCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateDelphiHexTemplateCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateDelphiHexTemplate(request.DelphiHexTemplate);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.DelphiHexTemplate), RuntimeInfo.DelphiHexTemplate).ConfigureAwait(false);
    }
}
