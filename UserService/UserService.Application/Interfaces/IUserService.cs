using UserService.Domain.Entities;
using UserService.Domain.Models;

namespace UserService.Application.Interfaces;

public interface IUserService
{
    Task<ViewUser> ProfileInfoUser(Guid id);
    Task<List<User>> GetAlLUser();
}