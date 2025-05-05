using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the HSL template for color output.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="HSLTemplate">The template string for HSL color output.</param>
public record UpdateHSLTemplateCommand(string HSLTemplate) : IRequest<RequestResultDto>;