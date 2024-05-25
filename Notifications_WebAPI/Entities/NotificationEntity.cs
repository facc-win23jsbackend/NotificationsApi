using System.ComponentModel.DataAnnotations;

namespace Notifications_WebAPI.Entities;

public class NotificationEntity
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public string? UserId { get; set; }
}
