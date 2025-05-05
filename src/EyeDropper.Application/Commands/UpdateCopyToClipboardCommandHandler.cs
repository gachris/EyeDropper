using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdateCopyToClipboardCommand"/>, updating the copy-to-clipboard setting.
/// </summary>
public class UpdateCopyToClipboardCommandHandler : UpdateSettingHandlerBase<UpdateCopyToClipboardCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCopyToClipboardCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdateCopyToClipboardCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdateCopyToClipboardCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdateCopyToClipboardCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdateCopyToClipboard(request.CopyToClipboard);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.CopyToClipboard), RuntimeInfo.CopyToClipboard).ConfigureAwait(false);
    }
}