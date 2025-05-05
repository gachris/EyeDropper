using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the VB-style hexadecimal template for color output.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="VBHexTemplate">The template string for VB hex color output.</param>
public record UpdateVBHexTemplateCommand(string VBHexTemplate) : IRequest<RequestResultDto>;
