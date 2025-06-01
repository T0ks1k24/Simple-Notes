namespace Domain.Entities;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTime Expiry { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}