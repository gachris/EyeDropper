using EyeDropper.ApplicationFlow;
using MediatR;
using MediatR.Pipeline;

namespace EyeDropper.Application;

/// <summary>
/// A MediatR post-processor that dispatches domain events after a request is handled.
/// </summary>
/// <typeparam name="TRequest">Type of the request.</typeparam>
/// <typeparam name="TResponse">Type of the response.</typeparam>
public class RequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    #region Fields/Consts

    private readonly IApplicationEventsDispatcher _dispatcher;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestPostProcessor{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="dispatcher">The events dispatcher used to dispatch domain events.</param>
    public RequestPostProcessor(IApplicationEventsDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    #region Methods

    /// <summary>
    /// Processes the request and response after the main handler has executed, dispatching any collected events.
    /// </summary>
    /// <param name="request">The request that was handled.</param>
    /// <param name="response">The response produced by the handler.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A completed task.</returns>
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        _dispatcher.Dispatch();
        return Unit.Task;
    }

    #endregion
}
