using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Api.Controllers;
[ApiController,Authorize(Roles="Admin"),Route("api/users")]
public class UsersController:ControllerBase
{
 private readonly IAdminService _service;public UsersController(IAdminService service)=>_service=service;
 private int UserId=>int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
 [HttpGet] public async Task<IActionResult> GetAll()=>Ok(await _service.GetUsersAsync());
 [HttpPut("{id:int}/role")] public async Task<IActionResult> Role(int id,UserDTO dto)=>Ok(await _service.UpdateRoleAsync(id,dto.Role,UserId));
 [HttpPut("{id:int}/status")] public async Task<IActionResult> Status(int id,UserDTO dto)=>Ok(await _service.UpdateStatusAsync(id,dto.IsActive,UserId));
}
