using Core.CryptoExchangeRate.Domain.Framework;
using MediatR;

namespace Core.CryptoExchangeRate.Application.Framework.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}