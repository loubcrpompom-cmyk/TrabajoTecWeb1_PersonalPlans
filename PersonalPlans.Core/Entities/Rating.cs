using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PersonalPlans.Core.Entities;

public class Rating : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

public int UserId { get; set; }
    public int ActivityId { get; set; }
    public int Score { get; set; }
public User? User { get; set; }
    public Activity? Activity { get; set; }
}

