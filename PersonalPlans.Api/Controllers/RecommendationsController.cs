using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces.Services;
using PersonalPlans.Services.Validators;

namespace PersonalPlans.Api.Controllers;

[ApiController, Authorize, Route("api/recommendations")]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationService _service;
    private readonly CrearRecommendationDtoValidator _crearValidator;

    public RecommendationsController(
        IRecommendationService service,
        CrearRecommendationDtoValidator crearValidator)
    {
        _service = service;
        _crearValidator = crearValidator;
    }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Generate(RecommendationDTO criteria)
    {
        // Usuario desde el token; Id y campos calculados los controla el servidor
        criteria.Id = 0;
        criteria.UserId = UserId;
        criteria.TotalCost = 0;
        criteria.TotalDurationMinutes = 0;
        criteria.RecommendationActivities = new();

        var validationResult = await _crearValidator.ValidateAsync(criteria);
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

        var r = await _service.GenerateAsync(UserId, criteria);
        return StatusCode(201, r);
    }

    [HttpGet]
    public async Task<IActionResult> History() =>
        Ok(await _service.GetHistoryAsync(UserId));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            throw new BusinessException("El ID de la recomendación debe ser mayor que cero.");
        return Ok(await _service.GetByIdAsync(UserId, id));
    }
}