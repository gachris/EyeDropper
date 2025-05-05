using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the application's color format setting.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="ColorFormat">The selected color format enumeration value.</param>
public record UpdateColorFormatCommand(int ColorFormat) : IRequest<RequestResultDto>;
