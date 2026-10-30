using PersonalPlans.Core.Entities;
namespace PersonalPlans.Core.Interfaces.Services;
public interface IUserService
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> RegisterAsync(string name, string email, string password);
    Task<User?> LoginAsync(string email, string password);
    Task UpdateAsync(User user);
}
