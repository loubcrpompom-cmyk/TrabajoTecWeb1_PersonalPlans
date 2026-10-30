namespace PersonalPlans.Core.DTOs;
public class RecommendationActivityDTO
{
    public int Id { get; set; }
    public int RecommendationId { get; set; }
    public int ActivityId { get; set; }
    public int Order { get; set; }
    public decimal Cost { get; set; }
    public int DurationMinutes { get; set; }
    public ActivityDTO? Activity { get; set; }
}
