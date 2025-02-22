using Core.CryptoExchangeRate.Domain.Framework;
using MediatR;

namespace Core.CryptoExchangeRate.Application.Framework.Queries;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}