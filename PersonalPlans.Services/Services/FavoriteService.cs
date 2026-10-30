using System.Net;
using AutoMapper;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Services.Services;
public class FavoriteService : IFavoriteService
{
 private readonly IFavoriteRepository _favorites;private readonly IActivityRepository _activities;private readonly IMapper _mapper;
 public FavoriteService(IFavoriteRepository favorites,IActivityRepository activities,IMapper mapper){_favorites=favorites;_activities=activities;_mapper=mapper;}
 public async Task<IEnumerable<FavoriteDTO>> GetByUserAsync(int userId)=>_mapper.Map<IEnumerable<FavoriteDTO>>(await _favorites.GetByUserIdAsync(userId));
 public async Task<FavoriteDTO> AddAsync(int userId,int activityId){var a=await _activities.GetByIdAsync(activityId);if(a is null||!a.IsActive)throw new BusinessException("Active activity not found.",HttpStatusCode.NotFound);if(await _favorites.GetByUserAndActivityAsync(userId,activityId)is not null)throw new BusinessException("This activity is already in favorites.",HttpStatusCode.Conflict);return _mapper.Map<FavoriteDTO>(await _favorites.CreateAsync(new Favorite{UserId=userId,ActivityId=activityId,CreatedAt=DateTime.UtcNow}));}
 public async Task DeleteAsync(int userId,int activityId){var f=await _favorites.GetByUserAndActivityAsync(userId,activityId);if(f is null)throw new BusinessException("Favorite not found.",HttpStatusCode.NotFound);await _favorites.DeleteAsync(f);}
}
