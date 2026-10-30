using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Api.Controllers;
[ApiController,Authorize,Route("api/activities/{activityId:int}/rating")]
public class RatingsController:ControllerBase
{
 private readonly IRatingService _service;public RatingsController(IRatingService service)=>_service=service;
 private int UserId=>int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
 [HttpPut] public async Task<IActionResult> Rate(int activityId,RatingDTO dto)=>Ok(await _service.RateAsync(UserId,activityId,dto.Score));
 [AllowAnonymous,HttpGet] public async Task<IActionResult> Average(int activityId)=>Ok(new{activityId,average=await _service.GetAverageAsync(activityId)});
}
