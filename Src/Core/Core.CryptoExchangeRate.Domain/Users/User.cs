using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.CryptoExchangeRate.Domain.Framework;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.CryptoExchangeRate.Domain.Users;

public class User : AggregateRoot<long>
{
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }

    public User()
    {
    }

    public User Create(IPasswordHasher<User> passwordHasher, string userName, string password)
    {
        Username = userName;
        PasswordHash = passwordHasher.HashPassword(this, password);

        return this;
    }

    public bool IsValid(IPasswordHasher<User> passwordHasher, string password)
    {
        var isValid = passwordHasher.VerifyHashedPassword(this, PasswordHash, password);

        return isValid == PasswordVerificationResult.Success;
    }
    
    public string GenerateJwtToken(IConfiguration configuration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: new[] { new Claim(ClaimTypes.Name, Username) },
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt16(configuration["Jwt:expire"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}