using EyeDropper.Application.Contracts;
using EyeDropper.Core.RuntimeOptions;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Handler for <see cref="UpdatePrecisionCommand"/>, updating the precision setting.
/// </summary>
public class UpdatePrecisionCommandHandler : UpdateSettingHandlerBase<UpdatePrecisionCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePrecisionCommandHandler"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    public UpdatePrecisionCommandHandler(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger<UpdatePrecisionCommandHandler> logger)
        : base(runtimeInfo, localSettingsService, logger) { }

    /// <inheritdoc />
    protected override async Task UpdateSettingAsync(UpdatePrecisionCommand request, CancellationToken cancellationToken)
    {
        RuntimeInfo.UpdatePrecision(request.Precision);
        await LocalSettingsService.SaveSettingAsync(nameof(IRuntimeOptionsState.Precision), RuntimeInfo.Precision).ConfigureAwait(false);
    }
}
