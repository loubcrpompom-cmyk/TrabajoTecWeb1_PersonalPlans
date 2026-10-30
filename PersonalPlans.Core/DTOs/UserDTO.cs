using System.ComponentModel.DataAnnotations;
namespace PersonalPlans.Core.DTOs;
public class UserDTO
{
    public int Id { get; set; }
    [StringLength(150)] public string Name { get; set; } = string.Empty;
    [EmailAddress, StringLength(255)] public string Email { get; set; } = string.Empty;
    // Used for registration/login requests; never returned by API responses.
    public string? Password { get; set; }
    public string Role { get; set; } = "User";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}
