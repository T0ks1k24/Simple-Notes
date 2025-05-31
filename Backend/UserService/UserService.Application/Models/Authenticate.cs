namespace UserService.Application.Models;

public class Authenticate
{
    public string Token { get; set; }
    public int ExpiresIn { get; set; }
}