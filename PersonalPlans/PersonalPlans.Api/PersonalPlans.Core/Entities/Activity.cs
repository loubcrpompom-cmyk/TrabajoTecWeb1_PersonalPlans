using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PersonalPlans.Core.Entities;

public class Activity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    // Cost per person, expressed in Bolivianos (Bs).
    public decimal CostPerPerson { get; set; }

    // Duration of the activity in minutes.
    public int DurationMinutes { get; set; }

    // Examples: Solo, Pareja, Amigos, Familia.
    public string SuitableCompanies { get; set; } = string.Empty;

    // False means the activity has been deactivated.
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

