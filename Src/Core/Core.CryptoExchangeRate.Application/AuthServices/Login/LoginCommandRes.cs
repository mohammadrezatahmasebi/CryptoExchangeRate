namespace Core.CryptoExchangeRate.Application.AuthServices.Login;

public class LoginCommandRes
{
    public LoginCommandRes(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
}