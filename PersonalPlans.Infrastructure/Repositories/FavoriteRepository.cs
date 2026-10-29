using Microsoft.EntityFrameworkCore;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Persistence;

namespace PersonalPlans.Infrastructure.Repositories;

public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(PersonalPlansContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(favorite => favorite.Activity)
            .Where(favorite => favorite.UserId == userId)
            .ToListAsync();
    }

    public async Task<Favorite?> GetByUserAndActivityAsync(int userId, int activityId)
    {
        return await _dbSet.FirstOrDefaultAsync(favorite =>
            favorite.UserId == userId && favorite.ActivityId == activityId);
    }
}
