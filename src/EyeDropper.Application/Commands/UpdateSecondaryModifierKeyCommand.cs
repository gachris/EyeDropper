using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the secondary modifier key for the color picker hotkey.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="SecondaryModifierKey">The secondary modifier key (e.g., Shift, Alt).</param>
public record UpdateSecondaryModifierKeyCommand(int SecondaryModifierKey) : IRequest<RequestResultDto>;