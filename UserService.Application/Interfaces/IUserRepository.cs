using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);
    Task<List<User>> GetByName(string namePart);
    Task<bool> CreateUser(User user);
}