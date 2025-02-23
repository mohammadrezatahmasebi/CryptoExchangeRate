using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.CryptoExchangeRate.Application.AuthServices.Contracts;
using Core.CryptoExchangeRate.Application.Framework.Commands;
using Core.CryptoExchangeRate.Domain.Framework;
using Core.CryptoExchangeRate.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.CryptoExchangeRate.Application.AuthServices.Login;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommandRequest, LoginCommandRes>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public Task<Result<LoginCommandRes>> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        var user = _userRepository.Get(request.UserName);

        if (user == null)
            return Task.FromResult(Result.Failure<LoginCommandRes>(UserError.NotFound));


        var isValid = user.IsValid(_passwordHasher, request.Password);


        if (!isValid)
            return Task.FromResult(Result.Failure<LoginCommandRes>(UserError.PasswordIsNotCorrect));


        var token = user.GenerateJwtToken(_configuration);

        return Task.FromResult(Result.Success(new LoginCommandRes(token)));
    }

  
}