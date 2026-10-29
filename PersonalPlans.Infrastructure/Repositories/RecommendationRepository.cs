using Microsoft.EntityFrameworkCore;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Persistence;

namespace PersonalPlans.Infrastructure.Repositories;

public class RecommendationRepository : BaseRepository<Recommendation>, IRecommendationRepository
{
    public RecommendationRepository(PersonalPlansContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Recommendation>> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(recommendation => recommendation.RecommendationActivities)
                .ThenInclude(recommendationActivity => recommendationActivity.Activity)
            .Where(recommendation => recommendation.UserId == userId)
            .OrderByDescending(recommendation => recommendation.CreatedAt)
            .ToListAsync();
    }

    public override async Task<Recommendation?> GetByIdAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(recommendation => recommendation.RecommendationActivities)
                .ThenInclude(recommendationActivity => recommendationActivity.Activity)
            .FirstOrDefaultAsync(recommendation => recommendation.Id == id);
    }
}
