using PersonalPlans.Core.DTOs;
namespace PersonalPlans.Core.Interfaces.Services;
public interface IRatingService { Task<RatingDTO> RateAsync(int userId,int activityId,int score); Task<double?> GetAverageAsync(int activityId); }
