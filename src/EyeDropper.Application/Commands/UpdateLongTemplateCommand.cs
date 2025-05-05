using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the long (comprehensive) color template for output.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="LongTemplate">The template string for the comprehensive color output.</param>
public record UpdateLongTemplateCommand(string LongTemplate) : IRequest<RequestResultDto>;
