using Core.CryptoExchangeRate.Domain.Framework;
using MediatR;

namespace Core.CryptoExchangeRate.Application.Framework.Commands;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand, IRequest<Result>
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>, IRequest<Result<TResponse>>
{
}