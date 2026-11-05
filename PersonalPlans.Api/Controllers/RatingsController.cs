using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces.Services;
using PersonalPlans.Services.Validators;

namespace PersonalPlans.Api.Controllers;

[ApiController, Authorize, Route("api/activities/{activityId:int}/rating")]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _service;
    private readonly CrearRatingDtoValidator _validator;

    public RatingsController(IRatingService service, CrearRatingDtoValidator validator)
    {
        _service = service;
        _validator = validator;
    }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPut]
    public async Task<IActionResult> Rate(int activityId, RatingDTO dto)
    {
        // Usuario y actividad no se toman del body: salen del token y de la ruta
        dto.Id = 0;
        dto.UserId = UserId;
        dto.ActivityId = activityId;

        var validationResult = await _validator.ValidateAsync(dto);
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

        return Ok(await _service.RateAsync(UserId, activityId, dto.Score));
    }

    [AllowAnonymous, HttpGet]
    public async Task<IActionResult> Average(int activityId) =>
        Ok(new { activityId, average = await _service.GetAverageAsync(activityId) });
}