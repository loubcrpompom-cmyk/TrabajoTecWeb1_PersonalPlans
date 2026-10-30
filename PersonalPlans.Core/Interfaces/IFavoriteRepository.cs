using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using PersonalPlans.Core.Entities;

namespace PersonalPlans.Core.Interfaces;

public interface IFavoriteRepository : IBaseRepository<Favorite>
{
    Task<IEnumerable<Favorite>> GetByUserIdAsync(int userId);

    Task<Favorite?> GetByUserAndActivityAsync(
        int userId,
        int activityId);
}
