using MediatR;

namespace DMT.Application.Common.CQRS;

public interface ICommand : ICommand<Unit> { }

public interface ICommand<out TResponse>
    : IRequest<TResponse>,
        ICommonOperationRequest<TResponse>
{ }
