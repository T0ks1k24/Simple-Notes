using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces;

public interface IUserService
{
    Task<ViewUser> ProfileInfoUser(Guid id);
    Task<List<User>> GetAlLUser();
}