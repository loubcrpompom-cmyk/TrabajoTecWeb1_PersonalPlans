using System.Net;
using AutoMapper;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Services.Services;
public class AdminService : IAdminService
{
 private readonly IUserRepository _users;private readonly IMapper _mapper;
 public AdminService(IUserRepository users,IMapper mapper){_users=users;_mapper=mapper;}
 public async Task<IEnumerable<UserDTO>> GetUsersAsync()=>_mapper.Map<IEnumerable<UserDTO>>(await _users.GetAllAsync());
 public async Task<UserDTO> UpdateRoleAsync(int id,string role,int actorId){if(role!="User"&&role!="Admin")throw new BusinessException("Role must be User or Admin.");var target=await _users.GetByIdAsync(id);if(target is null)throw new BusinessException("User not found.",HttpStatusCode.NotFound);var all=(await _users.GetAllAsync()).ToList();if(target.Role=="Admin"&&role!="Admin"&&target.IsActive&&all.Count(u=>u.Role=="Admin"&&u.IsActive)<=1)throw new BusinessException("At least one active administrator must remain.",HttpStatusCode.Conflict);target.Role=role;await _users.UpdateAsync(target);return _mapper.Map<UserDTO>(target);}
 public async Task<UserDTO> UpdateStatusAsync(int id,bool active,int actorId){var target=await _users.GetByIdAsync(id);if(target is null)throw new BusinessException("User not found.",HttpStatusCode.NotFound);var all=(await _users.GetAllAsync()).ToList();if(target.Id==actorId&&!active)throw new BusinessException("An administrator cannot deactivate their own account.",HttpStatusCode.Conflict);if(target.Role=="Admin"&&!active&&all.Count(u=>u.Role=="Admin"&&u.IsActive)<=1)throw new BusinessException("At least one active administrator must remain.",HttpStatusCode.Conflict);target.IsActive=active;await _users.UpdateAsync(target);return _mapper.Map<UserDTO>(target);}
}
