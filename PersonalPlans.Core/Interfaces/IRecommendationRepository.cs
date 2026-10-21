using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using PersonalPlans.Core.Entities;

namespace PersonalPlans.Core.Interfaces;

public interface IRecommendationRepository
{
    Task<IEnumerable<Recommendation>> GetByUserIdAsync(int userId);

    Task<Recommendation?> GetByIdAsync(int id);

    Task<Recommendation> CreateAsync(Recommendation recommendation);
}
