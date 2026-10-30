using System.ComponentModel.DataAnnotations;
namespace PersonalPlans.Core.DTOs;
public class RatingDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [Range(1, int.MaxValue)] public int ActivityId { get; set; }
    [Range(1, 5)] public int Score { get; set; }
    public DateTime CreatedAt { get; set; }
}
