using Microsoft.EntityFrameworkCore;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Persistence;

namespace PersonalPlans.Infrastructure.Repositories;

public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
{
    public ActivityRepository(PersonalPlansContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Activity>> GetActiveAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(activity => activity.IsActive)
            .ToListAsync();
    }

    public async Task<Activity?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(activity => activity.Name == name);
    }
}
