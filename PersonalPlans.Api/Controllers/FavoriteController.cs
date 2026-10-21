using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Repositories;

namespace PersonalPlans.Api.Controllers;

[ApiController]
[Route("api/users/{userId}/favorites")]
public class FavoritesController : ControllerBase
{
    private readonly Core.Interfaces.IFavoriteRepository _repository;

    public FavoritesController(Core.Interfaces.IFavoriteRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> GetByUser(int userId)
        => Ok(await _repository.GetByUserIdAsync(userId));

    [HttpPost("{activityId}")]
    public async Task<IActionResult> Add(int userId, int activityId)
    {
        var existing = await _repository.GetByUserAndActivityAsync(userId, activityId);
        if (existing is not null)
            return Conflict("La actividad ya está en favoritos.");

        var created = await _repository.CreateAsync(
            new Favorite { UserId = userId, ActivityId = activityId });

        return Created($"api/users/{userId}/favorites", created);
    }

    [HttpDelete("{activityId}")]
    public async Task<IActionResult> Remove(int userId, int activityId)
    {
        var existing = await _repository.GetByUserAndActivityAsync(userId, activityId);
        if (existing is null) return NotFound();

        await _repository.DeleteAsync(existing);
        return NoContent();
    }
}
