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
 private readonly IRatingRepository _ratings;private readonly IActivityRepository _activities;private readonly IRecommendationRepository _recommendations;private readonly IMapper _mapper;
 public RatingService(IRatingRepository ratings,IActivityRepository activities,IRecommendationRepository recommendations,IMapper mapper){_ratings=ratings;_activities=activities;_recommendations=recommendations;_mapper=mapper;}
 public async Task<RatingDTO> RateAsync(int userId,int activityId,int score){if(score<1||score>5)throw new BusinessException("Score must be between 1 and 5.");var activity=await _activities.GetByIdAsync(activityId);if(activity is null)throw new BusinessException("Activity not found.",HttpStatusCode.NotFound);var history=await _recommendations.GetByUserIdAsync(userId);if(!history.SelectMany(r=>r.RecommendationActivities).Any(ra=>ra.ActivityId==activityId))throw new BusinessException("You can only rate an activity previously recommended to you.",HttpStatusCode.Conflict);var existing=await _ratings.GetByUserAndActivityAsync(userId,activityId);if(existing is null)return _mapper.Map<RatingDTO>(await _ratings.CreateAsync(new Rating{UserId=userId,ActivityId=activityId,Score=score,CreatedAt=DateTime.UtcNow}));existing.Score=score;existing.CreatedAt=DateTime.UtcNow;await _ratings.UpdateAsync(existing);return _mapper.Map<RatingDTO>(existing);}
 public async Task<double?> GetAverageAsync(int activityId){if(await _activities.GetByIdAsync(activityId)is null)throw new BusinessException("Activity not found.",HttpStatusCode.NotFound);var ratings=await _ratings.GetByActivityIdAsync(activityId);return ratings.Any()?ratings.Average(r=>r.Score):null;}
}
