using MediatR;

namespace DMT.Application.Common.CQRS;

public interface IQuery<out T> : IRequest<T>, ICommonOperationRequest<T>
    where T : notnull
{ }
