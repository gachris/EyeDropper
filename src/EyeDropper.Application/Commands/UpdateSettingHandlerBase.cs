using DevToolbox.Core.Contracts;
using EyeDropper.Application.Dtos;
using EyeDropper.Core.RuntimeOptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Base class for handlers that update individual runtime settings.
/// Provides logging, event raising, and error handling around the update operation.
/// </summary>
/// <typeparam name="TCommand">The specific command type to handle.</typeparam>
public abstract class UpdateSettingHandlerBase<TCommand> : IRequestHandler<TCommand, RequestResultDto>
    where TCommand : IRequest<RequestResultDto>
{
    #region Fields/Consts

    private readonly ILogger _logger;

    /// <summary>
    /// Runtime options state to be updated.
    /// </summary>
    protected readonly IRuntimeOptionsState RuntimeInfo;

    /// <summary>
    /// Service for persisting local settings.
    /// </summary>
    protected readonly ILocalSettingsService LocalSettingsService;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSettingHandlerBase{TCommand}"/> class.
    /// </summary>
    /// <param name="runtimeInfo">The runtime options state to update.</param>
    /// <param name="localSettingsService">The service for saving settings.</param>
    /// <param name="events">The application events dispatcher.</param>
    /// <param name="logger">The logger for diagnostic messages.</param>
    protected UpdateSettingHandlerBase(
        IRuntimeOptionsState runtimeInfo,
        ILocalSettingsService localSettingsService,
        ILogger logger)
    {
        RuntimeInfo = runtimeInfo;
        LocalSettingsService = localSettingsService;
        _logger = logger;
    }

    #region Methods

    /// <inheritdoc />
    public async Task<RequestResultDto> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var settingName = typeof(TCommand).Name.Replace("Command", string.Empty);
        _logger.LogInformation("Starting update of setting {Setting}", settingName);

        try
        {
            await UpdateSettingAsync(request, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Successfully updated setting {Setting}", settingName);
            return new RequestResultDto();
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Update of setting {Setting} was canceled.", settingName);
            throw;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating setting {Setting}.", settingName);
            return new RequestResultDto($"Unable to update setting '{settingName}'", ex);
        }
    }

    /// <summary>
    /// Applies the new value from the command to the runtime options and ensures the change is persisted.
    /// </summary>
    /// <param name="request">The incoming command carrying the updated setting value.</param>
    /// <param name="cancellationToken">A token that can cancel the operation.</param>
    protected abstract Task UpdateSettingAsync(TCommand request, CancellationToken cancellationToken);

    #endregion
}