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

public class RecommendationRepository : IRecommendationRepository
{
    private readonly PersonalPlansContext _context;

    public RecommendationRepository(PersonalPlansContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Recommendation>> GetByUserIdAsync(
        int userId)
    {
        return await _context.Recommendations
            .AsNoTracking()
            .Include(r => r.RecommendationActivities)
                .ThenInclude(ra => ra.Activity)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Recommendation?> GetByIdAsync(int id)
    {
        return await _context.Recommendations
            .AsNoTracking()
            .Include(r => r.RecommendationActivities)
                .ThenInclude(ra => ra.Activity)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Recommendation> CreateAsync(
        Recommendation recommendation)
    {
        await _context.Recommendations.AddAsync(recommendation);
        await _context.SaveChangesAsync();

        return recommendation;
    }
}

