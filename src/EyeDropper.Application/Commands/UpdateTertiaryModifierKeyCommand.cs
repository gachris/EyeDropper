using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the tertiary modifier key for the color picker hotkey.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="TertiaryModifierKey">The tertiary modifier key (e.g., Windows, Control).</param>
public record UpdateTertiaryModifierKeyCommand(int TertiaryModifierKey) : IRequest<RequestResultDto>;
