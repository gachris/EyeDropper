using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the primary modifier key for the color picker hotkey.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="PrimaryModifierKey">The primary modifier key (e.g., Control, Alt).</param>
public record UpdatePrimaryModifierKeyCommand(int PrimaryModifierKey) : IRequest<RequestResultDto>;
