using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("user_name")]
    public string? Username { get; set; }
    [Column("email")]
    public string? Email { get; set; }
    [Column("password")]
    public string? Password { get; set; }
    [Column("role")]
    public string Role { get; set; } = "User";
}