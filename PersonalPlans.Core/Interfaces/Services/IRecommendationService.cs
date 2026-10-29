using PersonalPlans.Core.DTOs;
namespace PersonalPlans.Core.Interfaces.Services;
public interface IRecommendationService { Task<RecommendationDTO> GenerateAsync(int userId, RecommendationDTO criteria); Task<IEnumerable<RecommendationDTO>> GetHistoryAsync(int userId); Task<RecommendationDTO> GetByIdAsync(int userId,int id); }
