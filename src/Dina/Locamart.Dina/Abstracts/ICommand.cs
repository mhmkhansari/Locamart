using MediatR;

namespace Locamart.Dina.Abstracts;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
