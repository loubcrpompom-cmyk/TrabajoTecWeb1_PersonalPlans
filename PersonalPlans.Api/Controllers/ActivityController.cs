using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Repositories;

namespace PersonalPlans.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly Core.Interfaces.IActivityRepository _service;

    public ActivitiesController(Core.Interfaces.IActivityRepository service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("active")]
    public async Task<IActionResult> GetActive() => Ok(await _service.GetActiveAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var activity = await _service.GetByIdAsync(id);
        return activity is null ? NotFound() : Ok(activity);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Activity activity)
    {
        try
        {
            var created = await _service.CreateAsync(activity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Activity activity)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.Name = activity.Name;
        await _service.UpdateAsync(existing);
        return NoContent();
    }
}