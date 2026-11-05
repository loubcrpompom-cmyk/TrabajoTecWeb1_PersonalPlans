using System.Net;
using AutoMapper;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Services.Services;

public class RatingService : IRatingService
{
    private readonly IBaseRepository<Rating> _ratings;
    private readonly IBaseRepository<Activity> _activities;
    private readonly IBaseRepository<Recommendation> _recommendations;
    private readonly IMapper _mapper;

    public RatingService(
        IBaseRepository<Rating> ratings,
        IBaseRepository<Activity> activities,
        IBaseRepository<Recommendation> recommendations,
        IMapper mapper)
    {
        _ratings = ratings;
        _activities = activities;
        _recommendations = recommendations;
        _mapper = mapper;
    }

    public async Task<RatingDTO> RateAsync(int userId, int activityId, int score)
    {
        if (score < 1 || score > 5)
        {
            throw new BusinessException("Score must be between 1 and 5.");
        }

        var activity = await _activities.GetByIdAsync(activityId);

        if (activity is null)
        {
            throw new BusinessException("Activity not found.", HttpStatusCode.NotFound);
        }

        var allRecommendations = await _recommendations.GetAllAsync();
        var history = allRecommendations.Where(r => r.UserId == userId);

        if (!history.SelectMany(r => r.RecommendationActivities).Any(ra => ra.ActivityId == activityId))
        {
            throw new BusinessException("You can only rate an activity previously recommended to you.", HttpStatusCode.Conflict);
        }

        var allRatings = await _ratings.GetAllAsync();
        var existing = allRatings.FirstOrDefault(r => r.UserId == userId && r.ActivityId == activityId);

        if (existing is null)
        {
            return _mapper.Map<RatingDTO>(await _ratings.CreateAsync(new Rating { UserId = userId, ActivityId = activityId, Score = score, CreatedAt = DateTime.UtcNow }));
        }

        existing.Score = score;
        existing.CreatedAt = DateTime.UtcNow;
        await _ratings.UpdateAsync(existing);
        return _mapper.Map<RatingDTO>(existing);
    }

    public async Task<double?> GetAverageAsync(int activityId)
    {
        if (await _activities.GetByIdAsync(activityId) is null)
        {
            throw new BusinessException("Activity not found.", HttpStatusCode.NotFound);
        }

        var all = await _ratings.GetAllAsync();
        var ratings = all.Where(r => r.ActivityId == activityId).ToList();
        return ratings.Any() ? ratings.Average(r => r.Score) : null;
    }
}