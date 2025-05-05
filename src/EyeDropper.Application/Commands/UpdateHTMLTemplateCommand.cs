using EyeDropper.Application.Dtos;
using MediatR;

namespace EyeDropper.Application.Commands;

/// <summary>
/// Command to update the HTML template for color output.
/// </summary>
/// <remarks>
/// Implements <see cref="IRequest{RequestResultDto}"/>, returning a <see cref="RequestResultDto"/>
/// indicating the success or failure of the update operation.
/// </remarks>
/// <param name="HTMLTemplate">The template string for HTML color output.</param>
public record UpdateHTMLTemplateCommand(string HTMLTemplate) : IRequest<RequestResultDto>;