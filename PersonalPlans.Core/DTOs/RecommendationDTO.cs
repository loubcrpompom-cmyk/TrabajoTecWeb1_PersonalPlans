using System.ComponentModel.DataAnnotations;
namespace PersonalPlans.Core.DTOs;
public class RecommendationDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [Range(typeof(decimal), "0", "10000")] public decimal Budget { get; set; }
    [Range(1, 12)] public int AvailableTimeHours { get; set; }
    [Required] public string Company { get; set; } = string.Empty;
    [Range(1, 10)] public int NumberOfPeople { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalCost { get; set; }
    public int TotalDurationMinutes { get; set; }
    public List<RecommendationActivityDTO> RecommendationActivities { get; set; } = new();
}
