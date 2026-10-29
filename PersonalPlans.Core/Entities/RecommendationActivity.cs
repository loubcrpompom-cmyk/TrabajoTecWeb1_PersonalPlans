using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PersonalPlans.Core.Entities;

public class RecommendationActivity : BaseEntity
{
public int RecommendationId { get; set; }

    public int ActivityId { get; set; }

    // Position of the activity in the suggested plan (1, 2 or 3).
    public int Order { get; set; }

    // Cost and duration when the recommendation was generated.
    public decimal Cost { get; set; }

    public int DurationMinutes { get; set; }

    // Navigation properties
    public Recommendation? Recommendation { get; set; }

    public Activity? Activity { get; set; }
}

