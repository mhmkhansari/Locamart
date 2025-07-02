using MediatR;

namespace Locamart.Shared.Abstracts;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

