using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces.Services;
using PersonalPlans.Services.Validators;

namespace PersonalPlans.Api.Controllers;

[ApiController, Route("api/activities")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityService _service;
    private readonly IRatingService _ratings;
    private readonly CrearActivityDtoValidator _crearValidator;
    private readonly ActualizarActivityDtoValidator _actualizarValidator;

    public ActivitiesController(
        IActivityService service,
        IRatingService ratings,
        CrearActivityDtoValidator crearValidator,
        ActualizarActivityDtoValidator actualizarValidator)
    {
        _service = service;
        _ratings = ratings;
        _crearValidator = crearValidator;
        _actualizarValidator = actualizarValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync(User.IsInRole("Admin")));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var a = await _service.GetByIdAsync(id);
        var avg = await _ratings.GetAverageAsync(id);
        return Ok(new { activity = a, averageRating = avg });
    }

    [Authorize(Roles = "Admin"), HttpPost]
    public async Task<IActionResult> Create(ActivityDTO dto)
    {
        var validationResult = await _crearValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new BusinessException(
                message: "Error de validación",
                details: validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
        }

        var a = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = a.Id }, a);
    }

    [Authorize(Roles = "Admin"), HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ActivityDTO dto)
    {
        if (id != dto.Id)
            throw new BusinessException("El ID de la actividad no coincide.");
        var validationResult = await _actualizarValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            throw new BusinessException(
                message: "Error de validación",
                details: validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
        }

        return Ok(await _service.UpdateAsync(id, dto));
    }

    [Authorize(Roles = "Admin"), HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}