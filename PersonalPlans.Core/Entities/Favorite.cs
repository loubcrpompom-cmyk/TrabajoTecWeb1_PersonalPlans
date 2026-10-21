using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PersonalPlans.Core.Entities;

public class Favorite
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ActivityId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User? User { get; set; }

    public Activity? Activity { get; set; }
}

