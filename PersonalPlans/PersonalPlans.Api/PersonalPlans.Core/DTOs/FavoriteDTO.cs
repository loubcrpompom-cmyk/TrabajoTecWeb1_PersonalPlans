namespace PersonalPlans.Core.DTOs;
public class FavoriteDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ActivityId { get; set; }
    public DateTime CreatedAt { get; set; }
    public ActivityDTO? Activity { get; set; }
}
