using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notifications_WebAPI.Contexts;
using Notifications_WebAPI.Entities;

namespace Notifications_WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly NotificationContext _context;

    public NotificationsController(NotificationContext context)
    {
        _context = context;
    }



    [HttpPost] // CREATE - Skapar en notifikation
    public async Task<ActionResult<NotificationEntity>> CreateNotification([FromBody] NotificationEntity notification)
    {
        if (_context.Notifications.Any(x => x.Email == notification.Email))
        {
            return BadRequest("This email address is already subscribed");
        }

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOne), new { email = notification.Email }, notification);
    }



    [HttpGet] // READ - Hämtar alla Notifikationer till en lista
    public async Task<ActionResult<IEnumerable<NotificationEntity>>> GetAll()
    {
        return await _context.Notifications.ToListAsync();
    }



    [HttpGet("{email}")] // READ - Hämtar en Notifikation med email
    public async Task<ActionResult<NotificationEntity>> GetOne(string email)
    {
        NotificationEntity? notificationEntity = await _context.Notifications.FirstOrDefaultAsync(x => x.Email == email);
        return notificationEntity!;
    }



    [HttpPut("{email}")] // UPDATE - Uppdaterar en Notifikation entitet med email
    public async Task<IActionResult> UpdateOne(string email, [FromBody] NotificationEntity updatedNotification)
    {
        var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Email == email);
        if (notification == null)
        {
            return NotFound($"No notification with email address: {email}, was found.");
        }

        // Uppdaterar både e-post och prenumerationsstatus
        notification.Email = updatedNotification.Email;
        notification.IsActive = updatedNotification.IsActive; // Uppdaterar status

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Notifications.Any(x => x.Email == email))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }




    [HttpDelete("{email}")] // DELETE - Radera en notifikation med email
    public async Task<IActionResult> DeleteOne(string email)
    {
        var notification = _context.Notifications.FirstOrDefault(x => x.Email == email);
        if ( notification == null)
        {
            return NotFound();
        }

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

