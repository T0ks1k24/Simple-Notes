using Domain.ViewModels;

namespace Application.Interfaces;

public interface IUserService
{
    Task<ViewUser> ProfileInfoUser(Guid id);
}