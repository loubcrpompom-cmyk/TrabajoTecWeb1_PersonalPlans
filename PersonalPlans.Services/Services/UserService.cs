using System.Net;
using System.Text.RegularExpressions;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Core.Interfaces.Services;

namespace PersonalPlans.Services.Services;

public class UserService : IUserService
{
    private const int MaxFailedLoginAttempts = 3;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    private readonly IUserRepository _users;

    public UserService(IUserRepository users) => _users = users;

    public Task<User?> GetByIdAsync(int id) => _users.GetByIdAsync(id);

    public Task<User?> GetByEmailAsync(string email) =>
        _users.GetByEmailAsync(email.Trim().ToLowerInvariant());

    public Task<IEnumerable<User>> GetAllAsync() => _users.GetAllAsync();

    public async Task<User> RegisterAsync(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("Name is required.");

        if (string.IsNullOrWhiteSpace(email) ||
            !Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new BusinessException("A valid email is required.");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8 ||
            !password.Any(char.IsLetter) || !password.Any(char.IsDigit))
            throw new BusinessException("Password must contain at least 8 characters, including a letter and a number.");

        email = email.Trim().ToLowerInvariant();

        // Friendly application-level validation. The unique database index is
        // the final protection against simultaneous duplicate registrations.
        if (await _users.GetByEmailAsync(email) is not null)
            throw new BusinessException("Email is already registered.", HttpStatusCode.Conflict);

        return await _users.CreateAsync(new User
        {
            Name = name.Trim(),
            Email = email,
            Password = password, // Plain text, as explicitly requested for this version.
            Role = "User",
            IsActive = true,
            FailedLoginAttempts = 0,
            LockoutEndUtc = null,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new BusinessException("Email and password are required.");

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var user = await _users.GetByEmailAsync(normalizedEmail);

        // Do not reveal whether an email exists.
        if (user is null)
            return null;

        var now = DateTime.UtcNow;

        // A lockout is active until its UTC expiration time.
        if (user.LockoutEndUtc.HasValue && user.LockoutEndUtc.Value > now)
        {
            var remaining = user.LockoutEndUtc.Value - now;
            var minutes = Math.Max(1, (int)Math.Ceiling(remaining.TotalMinutes));
            throw new BusinessException(
                $"Account temporarily locked due to repeated failed login attempts. Try again in approximately {minutes} minute(s).",
                HttpStatusCode.Locked);
        }

        // Expired lockout: clear the old counter before checking credentials.
        if (user.LockoutEndUtc.HasValue && user.LockoutEndUtc.Value <= now)
        {
            user.FailedLoginAttempts = 0;
            user.LockoutEndUtc = null;
        }

        // Plain-text comparison is retained because password hashing was
        // explicitly excluded from this implementation.
        if (!string.Equals(user.Password, password, StringComparison.Ordinal))
        {
            user.FailedLoginAttempts++;

            if (user.FailedLoginAttempts >= MaxFailedLoginAttempts)
                user.LockoutEndUtc = now.Add(LockoutDuration);

            await _users.UpdateAsync(user);
            return null;
        }

        // Successful login resets the consecutive-failure counter and lockout.
        user.FailedLoginAttempts = 0;
        user.LockoutEndUtc = null;
        await _users.UpdateAsync(user);

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var existing = await _users.GetByIdAsync(user.Id);
        if (existing is null)
            throw new BusinessException("User not found.", HttpStatusCode.NotFound);

        await _users.UpdateAsync(user);
    }
}
