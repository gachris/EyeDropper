using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the RGB template for color output.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="RGBTemplate">The template string for RGB color output.</param>
public record UpdateRGBTemplateCommand(string RGBTemplate) : IRequest<RequestResultDto>;