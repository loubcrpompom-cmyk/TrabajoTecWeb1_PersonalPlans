using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Repositories;

namespace PersonalPlans.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly Core.Interfaces.IRecommendationRepository _recommendationRepository;

    public RecommendationsController(Core.Interfaces.IRecommendationRepository recommendationRepository)
    {
        _recommendationRepository = recommendationRepository;
    }

    // GET: api/recommendations/user/2
    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<IEnumerable<Recommendation>>> GetByUserId(int userId)
    {
        var recommendations = await _recommendationRepository.GetByUserIdAsync(userId);
        return Ok(recommendations);
    }

    // GET: api/recommendations/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Recommendation>> GetById(int id)
    {
        var recommendation = await _recommendationRepository.GetByIdAsync(id);

        if (recommendation is null)
            return NotFound(new { message = $"No se encontró la recomendación con id {id}." });

        return Ok(recommendation);
    }

    // POST: api/recommendations
    [HttpPost]
    public async Task<ActionResult<Recommendation>> Create([FromBody] Recommendation recommendation)
    {
        var created = await _recommendationRepository.CreateAsync(recommendation);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}