using PersonalPlans.Core.DTOs;
namespace PersonalPlans.Core.Interfaces.Services;
public interface IFavoriteService { Task<IEnumerable<FavoriteDTO>> GetByUserAsync(int userId); Task<FavoriteDTO> AddAsync(int userId,int activityId); Task DeleteAsync(int userId,int activityId); }
