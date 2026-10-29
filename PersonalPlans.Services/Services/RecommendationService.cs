using System.Net;
using AutoMapper;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Services.Services;
public class RecommendationService : IRecommendationService
{
 private readonly IRecommendationRepository _recommendations;private readonly IActivityRepository _activities;private readonly IRatingRepository _ratings;private readonly IMapper _mapper;
 public RecommendationService(IRecommendationRepository recommendations,IActivityRepository activities,IRatingRepository ratings,IMapper mapper){_recommendations=recommendations;_activities=activities;_ratings=ratings;_mapper=mapper;}
 public async Task<RecommendationDTO> GenerateAsync(int userId,RecommendationDTO c){if(c.Budget<0||c.Budget>10000)throw new BusinessException("Budget must be between Bs 0 and Bs 10000.");if(c.AvailableTimeHours<1||c.AvailableTimeHours>12)throw new BusinessException("Available time must be between 1 and 12 hours.");var allowed=new[]{"Solo","Pareja","Amigos","Familia"};if(!allowed.Contains(c.Company,StringComparer.OrdinalIgnoreCase))throw new BusinessException("Company must be Solo, Pareja, Amigos or Familia.");c.Company=allowed.First(x=>x.Equals(c.Company,StringComparison.OrdinalIgnoreCase));if((c.Company=="Solo"&&c.NumberOfPeople!=1)||(c.Company=="Pareja"&&c.NumberOfPeople!=2)||((c.Company=="Amigos"||c.Company=="Familia")&&(c.NumberOfPeople<2||c.NumberOfPeople>10)))throw new BusinessException("Number of people does not match the selected company.");
 var active=(await _activities.GetActiveAsync()).Where(a=>a.SuitableCompanies.Split(',',StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries).Contains(c.Company,StringComparer.OrdinalIgnoreCase)).Where(a=>(c.Budget==0?a.CostPerPerson==0:a.CostPerPerson*c.NumberOfPeople<=c.Budget)&&a.DurationMinutes<=c.AvailableTimeHours*60).ToList();
 if(active.Count==0)throw new BusinessException("No activities match the provided criteria.",HttpStatusCode.NotFound);
 var scored=new List<(Activity a,double avg,double score)>();foreach(var a in active){var rs=await _ratings.GetByActivityIdAsync(a.Id);var avg=rs.Any()?rs.Average(r=>r.Score):0;var budgetUse=c.Budget==0?1:(double)(a.CostPerPerson*c.NumberOfPeople/c.Budget);var timeUse=(double)a.DurationMinutes/(c.AvailableTimeHours*60);scored.Add((a,avg,budgetUse+timeUse+avg/5));}
 var chosen=new List<Activity>();foreach(var item in scored.OrderByDescending(x=>x.score)){if(chosen.Count==3)break;if(chosen.All(a=>a.Id!=item.a.Id)&&chosen.Sum(a=>a.CostPerPerson*c.NumberOfPeople)+item.a.CostPerPerson*c.NumberOfPeople<=c.Budget&&chosen.Sum(a=>a.DurationMinutes)+item.a.DurationMinutes<=c.AvailableTimeHours*60)chosen.Add(item.a);}if(chosen.Count==0)throw new BusinessException("No plan could be generated for the provided criteria.",HttpStatusCode.NotFound);
 var rec=new Recommendation{UserId=userId,Budget=c.Budget,AvailableTimeHours=c.AvailableTimeHours,Company=c.Company,NumberOfPeople=c.NumberOfPeople,CreatedAt=DateTime.UtcNow,TotalCost=chosen.Sum(a=>a.CostPerPerson*c.NumberOfPeople),TotalDurationMinutes=chosen.Sum(a=>a.DurationMinutes)};for(int i=0;i<chosen.Count;i++)rec.RecommendationActivities.Add(new RecommendationActivity{ActivityId=chosen[i].Id,Order=i+1,Cost=chosen[i].CostPerPerson*c.NumberOfPeople,DurationMinutes=chosen[i].DurationMinutes});var saved=await _recommendations.CreateAsync(rec);var hydrated=await _recommendations.GetByIdAsync(saved.Id)??saved;return _mapper.Map<RecommendationDTO>(hydrated);}
 public async Task<IEnumerable<RecommendationDTO>> GetHistoryAsync(int userId)=>_mapper.Map<IEnumerable<RecommendationDTO>>(await _recommendations.GetByUserIdAsync(userId));
 public async Task<RecommendationDTO> GetByIdAsync(int userId,int id){var r=await _recommendations.GetByIdAsync(id);if(r is null||r.UserId!=userId)throw new BusinessException("Recommendation not found.",HttpStatusCode.NotFound);return _mapper.Map<RecommendationDTO>(r);}
}
