using Core.CryptoExchangeRate.Application.Framework.Commands;

namespace Core.CryptoExchangeRate.Application.AuthServices.Login;

public sealed record LoginCommandRequest : ICommand<LoginCommandRes>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}