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

public class ActivityRepository : IActivityRepository
{
    private readonly PersonalPlansContext _context;

    public ActivityRepository(PersonalPlansContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Activity>> GetAllAsync()
    {
        return await _context.Activities
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Activity>> GetActiveAsync()
    {
        return await _context.Activities
            .AsNoTracking()
            .Where(a => a.IsActive)
            .ToListAsync();
    }

    public async Task<Activity?> GetByIdAsync(int id)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Activity?> GetByNameAsync(string name)
    {
        return await _context.Activities
            .FirstOrDefaultAsync(a => a.Name == name);
    }

    public async Task<Activity> CreateAsync(Activity activity)
    {
        await _context.Activities.AddAsync(activity);
        await _context.SaveChangesAsync();

        return activity;
    }

    public async Task UpdateAsync(Activity activity)
    {
        _context.Activities.Update(activity);
        await _context.SaveChangesAsync();
    }
}

