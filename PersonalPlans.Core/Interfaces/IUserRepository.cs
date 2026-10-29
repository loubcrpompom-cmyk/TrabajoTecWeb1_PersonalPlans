using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using PersonalPlans.Core.Entities;

namespace PersonalPlans.Core.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{

    Task<User?> GetByEmailAsync(string email);
}
