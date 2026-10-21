using Microsoft.AspNetCore.Mvc;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Infrastructure.Repositories;

namespace PersonalPlans.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly Core.Interfaces.IUserRepository _userRepository;

    public UsersController(Core.Interfaces.IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    // GET: api/users/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return NotFound(new { message = $"No se encontró el usuario con id {id}." });

        return Ok(user);
    }

    // GET: api/users/by-email/correo@ejemplo.com
    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<User>> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
            return NotFound(new { message = $"No se encontró un usuario con el email '{email}'." });

        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<User>> Create([FromBody] User user)
    {
        var existing = await _userRepository.GetByEmailAsync(user.Email);

        if (existing is not null)
            return Conflict(new { message = $"Ya existe un usuario con el email '{user.Email}'." });

        var created = await _userRepository.CreateAsync(user);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/users/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] User user)
    {
        var existing = await _userRepository.GetByIdAsync(id);

        if (existing is null)
            return NotFound(new { message = $"No se encontró el usuario con id {id}." });

        // Evita que el email quede duplicado con otro usuario
        var sameEmail = await _userRepository.GetByEmailAsync(user.Email);
        if (sameEmail is not null && sameEmail.Id != id)
            return Conflict(new { message = $"Ya existe otro usuario con el email '{user.Email}'." });

        user.Id = id;
        await _userRepository.UpdateAsync(user);

        return NoContent();
    }
}