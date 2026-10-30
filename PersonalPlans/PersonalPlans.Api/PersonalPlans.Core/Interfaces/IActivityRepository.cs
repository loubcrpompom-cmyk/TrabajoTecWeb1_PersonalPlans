using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using PersonalPlans.Core.Entities;

namespace PersonalPlans.Core.Interfaces;

public interface IActivityRepository
{
    Task<IEnumerable<Activity>> GetAllAsync();

    Task<IEnumerable<Activity>> GetActiveAsync();

    Task<Activity?> GetByIdAsync(int id);

    Task<Activity?> GetByNameAsync(string name);

    Task<Activity> CreateAsync(Activity activity);

    Task UpdateAsync(Activity activity);
}
