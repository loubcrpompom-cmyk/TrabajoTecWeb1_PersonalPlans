using Microsoft.EntityFrameworkCore;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Persistence;
namespace PersonalPlans.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly PersonalPlansContext _context;
    public UserRepository(PersonalPlansContext context) => _context = context;
    public Task<User?> GetByIdAsync(int id) => _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    public Task<User?> GetByEmailAsync(string email) => _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.AsNoTracking().OrderBy(u => u.Id).ToListAsync();
    public async Task<User> CreateAsync(User user) { await _context.Users.AddAsync(user); await _context.SaveChangesAsync(); return user; }
    public async Task UpdateAsync(User user) { _context.Users.Update(user); await _context.SaveChangesAsync(); }
}
