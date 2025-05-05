using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the RGB float template for color output.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="RGBFloatTemplate">The template string for floating-point RGB output.</param>
public record UpdateRGBFloatTemplateCommand(string RGBFloatTemplate) : IRequest<RequestResultDto>;