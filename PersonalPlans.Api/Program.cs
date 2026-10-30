using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalPlans.Api.Filters;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Core.Interfaces.Services;
using PersonalPlans.Infrastructure.Persistence;
using PersonalPlans.Infrastructure.Repositories;
using PersonalPlans.Infrastructure.Mappings;
using PersonalPlans.Services.Services;
var builder=WebApplication.CreateBuilder(args);
var connectionString=builder.Configuration.GetConnectionString("DefaultConnection");
if(string.IsNullOrWhiteSpace(connectionString)||connectionString.Contains("YOUR_MYSQL_PASSWORD")) throw new InvalidOperationException("Set ConnectionStrings:DefaultConnection in appsettings.json before running.");
builder.Services.AddDbContext<PersonalPlansContext>(o=>o.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
var jwtKey=builder.Configuration["Jwt:Key"]??throw new InvalidOperationException("Jwt:Key is missing.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o=>{o.TokenValidationParameters=new TokenValidationParameters{ValidateIssuer=true,ValidateAudience=true,ValidateLifetime=true,ValidateIssuerSigningKey=true,ValidIssuer=builder.Configuration["Jwt:Issuer"],ValidAudience=builder.Configuration["Jwt:Issuer"],IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))};});
builder.Services.AddAuthorization();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUserRepository,UserRepository>();builder.Services.AddScoped<IActivityRepository,ActivityRepository>();builder.Services.AddScoped<IFavoriteRepository,FavoriteRepository>();builder.Services.AddScoped<IRecommendationRepository,RecommendationRepository>();builder.Services.AddScoped<IRatingRepository,RatingRepository>();
builder.Services.AddScoped<IUserService,UserService>();builder.Services.AddScoped<IActivityService,ActivityService>();builder.Services.AddScoped<IFavoriteService,FavoriteService>();builder.Services.AddScoped<IRecommendationService,RecommendationService>();builder.Services.AddScoped<IRatingService,RatingService>();builder.Services.AddScoped<IAdminService,AdminService>();
var app=builder.Build();app.UseMiddleware<ExceptionHandlingMiddleware>();if(app.Environment.IsDevelopment())app.MapOpenApi();app.UseHttpsRedirection();app.UseAuthentication();app.UseAuthorization();app.MapControllers();app.Run();
