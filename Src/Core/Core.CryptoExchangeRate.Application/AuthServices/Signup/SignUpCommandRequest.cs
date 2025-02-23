using Core.CryptoExchangeRate.Application.Framework.Commands;

namespace Core.CryptoExchangeRate.Application.AuthServices.Signup;

public sealed record SignUpCommandRequest : ICommand<SignupCommandRes>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}