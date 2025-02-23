using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.CryptoExchangeRate.Application.AuthServices.Contracts;
using Core.CryptoExchangeRate.Application.Framework.Commands;
using Core.CryptoExchangeRate.Domain.Framework;
using Core.CryptoExchangeRate.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Core.CryptoExchangeRate.Application.AuthServices.Signup;

public sealed class SignupCommandHandler : ICommandHandler<SignUpCommandRequest, SignupCommandRes>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public SignupCommandHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public Task<Result<SignupCommandRes>> Handle(SignUpCommandRequest request, CancellationToken cancellationToken)
    {
        var user = _userRepository.Get(request.UserName);

        if (user != null)
            return Task.FromResult(Result.Failure<SignupCommandRes>(UserError.Duplicate));


        _userRepository.Add(new User().Create(_passwordHasher, request.UserName, request.Password));
        

        return Task.FromResult(Result.Success(new SignupCommandRes()));
    }
}