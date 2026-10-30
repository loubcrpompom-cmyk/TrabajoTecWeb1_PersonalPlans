using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Api.Controllers;
[ApiController, Route("api/auth")]
public class AuthController : ControllerBase
{
 private readonly IUserService _users;private readonly IConfiguration _configuration;
 public AuthController(IUserService users,IConfiguration configuration){_users=users;_configuration=configuration;}
 [HttpPost("register")]
 public async Task<IActionResult> Register(UserDTO request){var u=await _users.RegisterAsync(request.Name,request.Email,request.Password??string.Empty);return StatusCode(201,new UserDTO{Id=u.Id,Name=u.Name,Email=u.Email,Role=u.Role,IsActive=u.IsActive,CreatedAt=u.CreatedAt});}
 [HttpPost("login")]
 public async Task<IActionResult> Login(UserDTO request){var u=await _users.LoginAsync(request.Email,request.Password??string.Empty);if(u is null)throw new BusinessException("Invalid credentials or inactive account.",System.Net.HttpStatusCode.Unauthorized);var key=_configuration["Jwt:Key"]??throw new InvalidOperationException("Jwt:Key is missing.");var issuer=_configuration["Jwt:Issuer"]??"PersonalPlans";var claims=new[]{new Claim(ClaimTypes.NameIdentifier,u.Id.ToString()),new Claim(ClaimTypes.Name,u.Email),new Claim(ClaimTypes.Role,u.Role)};var credentials=new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),SecurityAlgorithms.HmacSha256);var token=new JwtSecurityToken(issuer,issuer,claims,expires:DateTime.UtcNow.AddHours(8),signingCredentials:credentials);return Ok(new{token=new JwtSecurityTokenHandler().WriteToken(token),user=new UserDTO{Id=u.Id,Name=u.Name,Email=u.Email,Role=u.Role,IsActive=u.IsActive,CreatedAt=u.CreatedAt}});}
}
