using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateHTMLTemplateCommand"/>, updating the HTML template.
/// </summary>
public class UpdateHTMLTemplateCommandHandler : UpdateSettingHandlerBase<UpdateHTMLTemplateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateHTMLTemplateCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.
    /// </param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateHTMLTemplateCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateHTMLTemplateCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateHTMLTemplateCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateHTMLTemplate(request.HTMLTemplate);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.HTMLTemplate), RuntimeInfo.HTMLTemplate).ConfigureAwait(false);
    }
}
