using Application.Interfaces;
using Domain.Entities;
using Domain.ViewModels;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    //Method Returns Information About The User
    public async Task<ViewUser> ProfileInfoUser(Guid id)
    {
        var user = await _userRepository.GetUserById(id);
        var profileUser = new ViewUser
        {
            Email = user.Email,
            Name = user.Username,
            Role = user.Role
        };
        return profileUser;
    }
    

    
    
}