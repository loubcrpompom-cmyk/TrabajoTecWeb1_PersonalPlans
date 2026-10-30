using PersonalPlans.Core.DTOs;
namespace PersonalPlans.Core.Interfaces.Services;
public interface IAdminService { Task<IEnumerable<UserDTO>> GetUsersAsync(); Task<UserDTO> UpdateRoleAsync(int id,string role,int actorId); Task<UserDTO> UpdateStatusAsync(int id,bool active,int actorId); }
