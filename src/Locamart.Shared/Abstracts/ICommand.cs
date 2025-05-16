using MediatR;


namespace Locamart.Shared.Abstracts;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
