using PersonalPlans.Core.DTOs;
namespace PersonalPlans.Core.Interfaces.Services;
public interface IActivityService { Task<IEnumerable<ActivityDTO>> GetAllAsync(bool includeInactive=false); Task<ActivityDTO> GetByIdAsync(int id); Task<ActivityDTO> CreateAsync(ActivityDTO dto); Task<ActivityDTO> UpdateAsync(int id, ActivityDTO dto); Task DeleteAsync(int id); }
