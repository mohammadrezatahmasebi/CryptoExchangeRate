using Core.CryptoExchangeRate.Application.AuthServices.Contracts;
using Core.CryptoExchangeRate.Domain.Users;

namespace Infra.CryptoExchangeRate.Db;

public class UserRepository:IUserRepository
{
    private readonly Dictionary<string, User> _users;


    public UserRepository()
    {
        _users = [];
    }
    
    public User? Get(string userName)
    {
        return _users[userName];
    }

    public void Add(User user)
    {
        _users.Add(user.Username, user);
    }
}