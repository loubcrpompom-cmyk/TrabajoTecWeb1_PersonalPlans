using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Repositories;

namespace PersonalPlans.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController : ControllerBase
{
    private readonly Core.Interfaces.IRatingRepository _ratingRepository;

    public RatingsController(Core.Interfaces.IRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    // GET: api/ratings/activity/3
    [HttpGet("activity/{activityId:int}")]
    public async Task<ActionResult<IEnumerable<Rating>>> GetByActivityId(int activityId)
    {
        var ratings = await _ratingRepository.GetByActivityIdAsync(activityId);
        return Ok(ratings);
    }

    // GET: api/ratings/user/2/activity/3
    [HttpGet("user/{userId:int}/activity/{activityId:int}")]
    public async Task<ActionResult<Rating>> GetByUserAndActivity(int userId, int activityId)
    {
        var rating = await _ratingRepository.GetByUserAndActivityAsync(userId, activityId);

        if (rating is null)
            return NotFound(new { message = $"El usuario {userId} no ha calificado la actividad {activityId}." });

        return Ok(rating);
    }

    // POST: api/ratings
    [HttpPost]
    public async Task<ActionResult<Rating>> Create([FromBody] Rating rating)
    {
        // Un usuario solo puede calificar una vez cada actividad
        var existing = await _ratingRepository.GetByUserAndActivityAsync(rating.UserId, rating.ActivityId);

        if (existing is not null)
            return Conflict(new { message = "Este usuario ya calificó esta actividad. Usa PUT para actualizar la calificación." });

        var created = await _ratingRepository.CreateAsync(rating);

        return CreatedAtAction(
            nameof(GetByUserAndActivity),
            new { userId = created.UserId, activityId = created.ActivityId },
            created);
    }

    // PUT: api/ratings/user/2/activity/3
    [HttpPut("user/{userId:int}/activity/{activityId:int}")]
    public async Task<IActionResult> Update(int userId, int activityId, [FromBody] Rating rating)
    {
        var existing = await _ratingRepository.GetByUserAndActivityAsync(userId, activityId);

        if (existing is null)
            return NotFound(new { message = $"El usuario {userId} no ha calificado la actividad {activityId}." });

        rating.UserId = userId;
        rating.ActivityId = activityId;

        await _ratingRepository.UpdateAsync(rating);

        return NoContent();
    }
}
