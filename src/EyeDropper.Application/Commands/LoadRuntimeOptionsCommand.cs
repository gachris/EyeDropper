using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to read runtime settings from the application configuration store.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, where the result encapsulates
/// success status and loaded settings.</remarks>
public record LoadRuntimeOptionsCommand : IRequest<RequestResultDto>;
