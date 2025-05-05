using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the hexadecimal template for color output.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="HexTemplate">The template string for hexadecimal color output.</param>
public record UpdateHexTemplateCommand(string HexTemplate) : IRequest<RequestResultDto>;
