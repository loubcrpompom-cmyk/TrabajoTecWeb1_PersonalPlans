using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PersonalPlans.Core.Entities;

public class User : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public bool IsActive { get; set; } = true;

    // Consecutive failed login attempts. Reset after a successful login.
    public int FailedLoginAttempts { get; set; } = 0;

    // UTC timestamp until which login is blocked after three failed attempts.
    public DateTime? LockoutEndUtc { get; set; }
}
