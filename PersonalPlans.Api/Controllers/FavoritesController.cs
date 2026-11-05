using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces.Services;
using PersonalPlans.Services.Validators;

namespace PersonalPlans.Api.Controllers;

[ApiController, Authorize, Route("api/favorites")]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _service;
    private readonly CrearFavoriteDtoValidator _crearValidator;

    public FavoritesController(IFavoriteService service, CrearFavoriteDtoValidator crearValidator)
    {
        _service = service;
        _crearValidator = crearValidator;
    }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> Get() =>
        Ok(await _service.GetByUserAsync(UserId));

    [HttpPost]
    public async Task<IActionResult> Add(FavoriteDTO dto)
    {
        // El usuario siempre sale del token, nunca del body
        dto.UserId = UserId;

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

        var f = await _service.AddAsync(UserId, dto.ActivityId);
        return CreatedAtAction(nameof(Get), new { }, f);
    }

    [HttpDelete("{activityId:int}")]
    public async Task<IActionResult> Delete(int activityId)
    {
        if (activityId <= 0)
            throw new BusinessException("El ID de la actividad debe ser mayor que cero.");

        await _service.DeleteAsync(UserId, activityId);
        return NoContent();
    }
}