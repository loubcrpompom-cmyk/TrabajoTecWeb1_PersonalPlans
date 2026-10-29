using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Api.Controllers;
[ApiController,Authorize,Route("api/favorites")]
public class FavoritesController:ControllerBase
{
 private readonly IFavoriteService _service;public FavoritesController(IFavoriteService service)=>_service=service;
 private int UserId=>int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
 [HttpGet] public async Task<IActionResult> Get()=>Ok(await _service.GetByUserAsync(UserId));
 [HttpPost] public async Task<IActionResult> Add(FavoriteDTO dto){var f=await _service.AddAsync(UserId,dto.ActivityId);return CreatedAtAction(nameof(Get), new { }, f);}
 [HttpDelete("{activityId:int}")] public async Task<IActionResult> Delete(int activityId){await _service.DeleteAsync(UserId,activityId);return NoContent();}
}
