using System.ComponentModel.DataAnnotations;
namespace PersonalPlans.Core.DTOs;
public class ActivityDTO
{
    public int Id { get; set; }
    [StringLength(150)] public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required, StringLength(100)] public string Category { get; set; } = string.Empty;
    [Range(typeof(decimal), "0", "1000000")] public decimal CostPerPerson { get; set; }
    [Range(1, 1440)] public int DurationMinutes { get; set; }
    [Required] public string SuitableCompanies { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}
