using System.Net;
using AutoMapper;
using PersonalPlans.Core.DTOs;
using PersonalPlans.Core.Entities;
using PersonalPlans.Core.Exceptions;
using PersonalPlans.Core.Interfaces;
using PersonalPlans.Core.Interfaces.Services;
namespace PersonalPlans.Services.Services;

public class ActivityService : IActivityService
{
    private readonly IBaseRepository<Activity> _repo;
    private readonly IMapper _mapper;
    public ActivityService(
        IBaseRepository<Activity> repo,
        IMapper mapper)
    {
        _repo = repo; _mapper = mapper;
    }
    public async Task<IEnumerable<ActivityDTO>> GetAllAsync(bool includeInactive = false)
    {
        var all = await _repo.GetAllAsync();
        var items = includeInactive ? all : all.Where(a => a.IsActive);
        return _mapper.Map<IEnumerable<ActivityDTO>>(items);
    }

    public async Task<ActivityDTO> GetByIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        if (a is null) throw new BusinessException("Activity not found.", HttpStatusCode.NotFound);
        return _mapper.Map<ActivityDTO>(a);
    }
    public async Task<ActivityDTO> CreateAsync(ActivityDTO dto)
    {
        Validate(dto);
        if (await FindByNameAsync(dto.Name.Trim()) is not null) throw new BusinessException("An activity with this name already exists.",
            HttpStatusCode.Conflict);
        var a = _mapper.Map<Activity>(dto);
        a.Id = 0; a.Name = dto.Name.Trim();
        a.CreatedAt = DateTime.UtcNow; a.IsActive = true;
        return _mapper.Map<ActivityDTO>(await _repo.CreateAsync(a));
    }
    public async Task<ActivityDTO> UpdateAsync(int id, ActivityDTO dto)
    {
        Validate(dto); var a = await _repo.GetByIdAsync(id);

        if (a is null) throw new BusinessException("Activity not found.", HttpStatusCode.NotFound);

        var same = await FindByNameAsync(dto.Name.Trim());

        if (same is not null && same.Id != id)
        {
            throw new BusinessException("An activity with this name already exists.", HttpStatusCode.Conflict);
        }
        a.Name = dto.Name.Trim();
        a.Description = dto.Description;
        a.Category = dto.Category;
        a.CostPerPerson = dto.CostPerPerson;
        a.DurationMinutes = dto.DurationMinutes;
        a.SuitableCompanies = dto.SuitableCompanies;
        a.IsActive = dto.IsActive;
        await _repo.UpdateAsync(a);
        return _mapper.Map<ActivityDTO>(a);
    }

    public async Task DeleteAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        if (a is null)
        {
            throw new BusinessException("Activity not found.", HttpStatusCode.NotFound);
        }
        a.IsActive = false;
        await _repo.UpdateAsync(a);
    }

    private async Task<Activity?> FindByNameAsync(string name)
    {
        var all = await _repo.GetAllAsync();
        return all.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private static void Validate(ActivityDTO d)
    {
        if (string.IsNullOrWhiteSpace(d.Name) || string.IsNullOrWhiteSpace(d.Category) || string.IsNullOrWhiteSpace(d.SuitableCompanies))
        {
            throw new BusinessException("Name, category and suitable companies are required.");
        }
        if (d.CostPerPerson < 0)
        {
            throw new BusinessException("Cost per person cannot be negative.");
        }

        if (d.DurationMinutes <= 0)
        {
            throw new BusinessException("Duration must be greater than zero.");
        }

        var allowed = new[] { "Solo", "Pareja", "Amigos", "Familia" };

        var companies = d.SuitableCompanies.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (companies.Length == 0 || companies.Any(c => !allowed.Contains(c, StringComparer.OrdinalIgnoreCase)))
        {
            throw new BusinessException("SuitableCompanies must contain Solo, Pareja, Amigos and/or Familia separated by commas.");
        }

        d.SuitableCompanies = string.Join(',', companies.Select(c => allowed.First(x => x.Equals(c, StringComparison.OrdinalIgnoreCase))));
    }
}