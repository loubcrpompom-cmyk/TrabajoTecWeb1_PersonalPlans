using AutoMapper;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Entities;
namespace PersonalPlans.Services.Mapping;
public class MappingProfile : Profile
{
 public MappingProfile()
 {
  CreateMap<Activity, ActivityDTO>().ReverseMap();
  CreateMap<Favorite, FavoriteDTO>().ReverseMap();
  CreateMap<Rating, RatingDTO>().ReverseMap();
  CreateMap<RecommendationActivity, RecommendationActivityDTO>().ReverseMap();
  CreateMap<Recommendation, RecommendationDTO>().ReverseMap();
  CreateMap<User, UserDTO>().ForMember(d=>d.Password,o=>o.Ignore()).ReverseMap().ForMember(d=>d.Password,o=>o.MapFrom(s=>s.Password ?? string.Empty));
 }
}
