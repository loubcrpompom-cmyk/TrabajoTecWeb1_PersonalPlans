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

public class RatingRepository : IRatingRepository
{
    private readonly PersonalPlansContext _context;

    public RatingRepository(PersonalPlansContext context)
    {
        _context = context;
    }

    public async Task<Rating?> GetByUserAndActivityAsync(
        int userId,
        int activityId)
    {
        return await _context.Ratings
            .FirstOrDefaultAsync(r =>
                r.UserId == userId &&
                r.ActivityId == activityId);
    }

    public async Task<IEnumerable<Rating>> GetByActivityIdAsync(
        int activityId)
    {
        return await _context.Ratings
            .AsNoTracking()
            .Where(r => r.ActivityId == activityId)
            .ToListAsync();
    }

    public async Task<Rating> CreateAsync(Rating rating)
    {
        await _context.Ratings.AddAsync(rating);
        await _context.SaveChangesAsync();

        return rating;
    }

    public async Task UpdateAsync(Rating rating)
    {
        _context.Ratings.Update(rating);
        await _context.SaveChangesAsync();
    }
}
