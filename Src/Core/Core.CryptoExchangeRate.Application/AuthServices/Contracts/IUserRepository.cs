using Core.CryptoExchangeRate.Domain.Users;

namespace Core.CryptoExchangeRate.Application.AuthServices.Contracts;

public interface IUserRepository
{
    User? Get(string userName);


    void Add(User user);
}