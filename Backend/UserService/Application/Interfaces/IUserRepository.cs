using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);
    Task<List<User>> GetByName(string namePart);
    Task<bool> CreateUser(User user);
    Task<ViewUser> GetById(Guid id);
    Task<List<User>> GetAll();
}