using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Persistence;

namespace PersonalPlans.Infrastructure.Repositories;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly PersonalPlansContext _context;

    public FavoriteRepository(PersonalPlansContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId)
    {
        return await _context.Favorites
            .AsNoTracking()
            .Include(f => f.Activity)
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    public async Task<Favorite?> GetByUserAndActivityAsync(
        int userId,
        int activityId)
    {
        return await _context.Favorites
            .FirstOrDefaultAsync(f =>
                f.UserId == userId &&
                f.ActivityId == activityId);
    }

    public async Task<Favorite> CreateAsync(Favorite favorite)
    {
        await _context.Favorites.AddAsync(favorite);
        await _context.SaveChangesAsync();

        return favorite;
    }

    public async Task DeleteAsync(Favorite favorite)
    {
        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
    }
}
