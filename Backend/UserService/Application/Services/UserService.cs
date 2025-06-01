using Application.Interfaces;
using Domain.Entities;
using Domain.Models;

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
        var user = await _userRepository.GetById(id);
        return user;
    }

    //Method Returns All Users 
    public async Task<List<User>> GetAlLUser()
    {
        return await _userRepository.GetAll();
    }
    
}