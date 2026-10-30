using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Api.Controllers;
[ApiController,Authorize,Route("api/recommendations")]
public class RecommendationsController:ControllerBase
{
 private readonly IRecommendationService _service;public RecommendationsController(IRecommendationService service)=>_service=service;
 private int UserId=>int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
 [HttpPost] public async Task<IActionResult> Generate(RecommendationDTO criteria){var r=await _service.GenerateAsync(UserId,criteria);return StatusCode(201,r);}
 [HttpGet] public async Task<IActionResult> History()=>Ok(await _service.GetHistoryAsync(UserId));
 [HttpGet("{id:int}")] public async Task<IActionResult> GetById(int id)=>Ok(await _service.GetByIdAsync(UserId,id));
}
