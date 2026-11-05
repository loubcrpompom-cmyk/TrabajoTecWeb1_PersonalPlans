using System.Net;
using AutoMapper;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Services.Services;

public class FavoriteService : IFavoriteService
{
    private readonly IBaseRepository<Favorite> _favorites;
    private readonly IBaseRepository<Activity> _activities;
    private readonly IMapper _mapper;

    public FavoriteService(IBaseRepository<Favorite> favorites, IBaseRepository<Activity> activities, IMapper mapper)
    {
        _favorites = favorites;
        _activities = activities;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FavoriteDTO>> GetByUserAsync(int userId)
    {
        var all = await _favorites.GetAllAsync();
        var items = all.Where(f => f.UserId == userId);
        return _mapper.Map<IEnumerable<FavoriteDTO>>(items);
    }

    public async Task<FavoriteDTO> AddAsync(int userId, int activityId)
    {
        var a = await _activities.GetByIdAsync(activityId);
        if (a is null || !a.IsActive)
        {
            throw new BusinessException("Active activity not found.", HttpStatusCode.NotFound);
        }
        if (await FindByUserAndActivityAsync(userId, activityId) is not null)
        {
            throw new BusinessException("This activity is already in favorites.", HttpStatusCode.Conflict);
        }
        return _mapper.Map<FavoriteDTO>(await _favorites.CreateAsync(new Favorite { UserId = userId, ActivityId = activityId, CreatedAt = DateTime.UtcNow }));
    }

    public async Task DeleteAsync(int userId, int activityId)
    {
        var f = await FindByUserAndActivityAsync(userId, activityId);

        if (f is null)
        {
            throw new BusinessException("Favorite not found.", HttpStatusCode.NotFound);
        }

        await _favorites.DeleteAsync(f);
    }

    private async Task<Favorite?> FindByUserAndActivityAsync(int userId, int activityId)
    {
        var all = await _favorites.GetAllAsync();
        return all.FirstOrDefault(f => f.UserId == userId && f.ActivityId == activityId);
    }
}