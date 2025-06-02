using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUser();
    Task<User> GetUserById(Guid id);
    Task<User?> GetUserByEmail(string email);
    Task<List<User>> GetUserByName(string namePart);
    Task<bool> CreateUser(User user);
    Task<bool> UpdateUser(User user);
    Task DeleteUser(User user);
    
    
}