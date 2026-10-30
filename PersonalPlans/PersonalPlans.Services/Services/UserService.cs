using System.Net;
using System.Text.RegularExpressions;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Services.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _users;
    public UserService(IUserRepository users) => _users = users;
    public Task<User?> GetByIdAsync(int id) => _users.GetByIdAsync(id);
    public Task<User?> GetByEmailAsync(string email) => _users.GetByEmailAsync(email.Trim().ToLowerInvariant());
    public Task<IEnumerable<User>> GetAllAsync() => _users.GetAllAsync();
    public async Task<User> RegisterAsync(string name, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new BusinessException("Name is required.");
        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) throw new BusinessException("A valid email is required.");
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8 || !password.Any(char.IsLetter) || !password.Any(char.IsDigit)) throw new BusinessException("Password must contain at least 8 characters, including a letter and a number.");
        email = email.Trim().ToLowerInvariant();
        if (await _users.GetByEmailAsync(email) is not null) throw new BusinessException("Email is already registered.", HttpStatusCode.Conflict);
        return await _users.CreateAsync(new User { Name=name.Trim(), Email=email, Password=password, Role="User", IsActive=true, CreatedAt=DateTime.UtcNow });
    }
    public async Task<User?> LoginAsync(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) throw new BusinessException("Email and password are required.");
        var user = await _users.GetByEmailAsync(email.Trim().ToLowerInvariant());
        if (user is null || !user.IsActive || user.Password != password) return null;
        return user;
    }
    public async Task UpdateAsync(User user)
    {
        var existing = await _users.GetByIdAsync(user.Id);
        if (existing is null) throw new BusinessException("User not found.", HttpStatusCode.NotFound);
        await _users.UpdateAsync(user);
    }
}
