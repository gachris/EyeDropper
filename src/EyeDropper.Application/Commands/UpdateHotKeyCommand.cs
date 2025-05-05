using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the main hotkey for activating the color picker.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="HotKey">
/// The key code used in combination with modifier keys to trigger the color picker.
/// </param>
public record UpdateHotKeyCommand(int HotKey) : IRequest<RequestResultDto>;