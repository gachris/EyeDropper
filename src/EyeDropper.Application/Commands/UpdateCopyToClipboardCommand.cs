using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the application's "copy to clipboard" setting.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="CopyToClipboard">
/// True to enable automatic copying of captured colors to the clipboard; otherwise, false.
/// </param>
public record UpdateCopyToClipboardCommand(bool CopyToClipboard) : IRequest<RequestResultDto>;