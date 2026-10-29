using Microsoft.EntityFrameworkCore;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Persistence;

namespace PersonalPlans.Infrastructure.Repositories;

public class RatingRepository : BaseRepository<Rating>, IRatingRepository
{
    public RatingRepository(PersonalPlansContext context) : base(context)
    {
    }

    public async Task<Rating?> GetByUserAndActivityAsync(int userId, int activityId)
    {
        return await _dbSet.FirstOrDefaultAsync(rating =>
            rating.UserId == userId && rating.ActivityId == activityId);
    }

    public async Task<IEnumerable<Rating>> GetByActivityIdAsync(int activityId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(rating => rating.ActivityId == activityId)
            .ToListAsync();
    }
}
