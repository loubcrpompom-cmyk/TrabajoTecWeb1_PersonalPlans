using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PersonalPlans.Core.Entities;

public class Recommendation : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

public int UserId { get; set; }

    public decimal Budget { get; set; }

    public int AvailableTimeHours { get; set; }

    public string Company { get; set; } = string.Empty;

    public int NumberOfPeople { get; set; }
public decimal TotalCost { get; set; }

    public int TotalDurationMinutes { get; set; }

    // Navigation properties
    public User? User { get; set; }

    public ICollection<RecommendationActivity> RecommendationActivities
    { get; set; } = new List<RecommendationActivity>();
}
