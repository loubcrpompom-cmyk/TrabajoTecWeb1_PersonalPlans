using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using PersonalPlans.Core.Entities;

namespace PersonalPlans.Core.Interfaces;

public interface IRatingRepository : IBaseRepository<Rating>
{
    Task<Rating?> GetByUserAndActivityAsync(int userId, int activityId);

    Task<IEnumerable<Rating>> GetByActivityIdAsync(int activityId);
}

