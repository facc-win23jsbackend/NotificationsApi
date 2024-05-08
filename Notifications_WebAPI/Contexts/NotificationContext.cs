using Microsoft.EntityFrameworkCore;
using Notifications_WebAPI.Entities;

namespace Notifications_WebAPI.Contexts;

public class NotificationContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<NotificationEntity> Notifications { get; set; }
}
