using MediatR;

namespace Locamart.Dina.Abstracts;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

