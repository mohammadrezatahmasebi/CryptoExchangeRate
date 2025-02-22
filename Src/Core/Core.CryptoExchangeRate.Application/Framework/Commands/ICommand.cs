using Core.CryptoExchangeRate.Domain.Framework;
using MediatR;

namespace Core.CryptoExchangeRate.Application.Framework.Commands;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TReponse> : IRequest<Result<TReponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
}