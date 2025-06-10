namespace Domain.Models.JwtTokenApi;

public class JwtToken
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}