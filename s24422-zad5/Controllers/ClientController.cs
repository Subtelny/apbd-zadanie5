using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using s24422_zad5.Dtos;
using s24422_zad5.Models;

namespace s24422_zad5.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly TripContext _context;

    public ClientsController(TripContext context)
    {
        _context = context;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }

        var clientTrips = await _context.ClientTrips.Where(ct => ct.ClientId == id).ToListAsync();
        if (clientTrips.Any())
        {
            return BadRequest("Client has assigned trips and cannot be deleted.");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("trips/{tripId}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int tripId, [FromBody] ClientTripDTO clientTripDTO)
    {
        var client = await _context.Clients.SingleOrDefaultAsync(c => c.Pesel == clientTripDTO.Pesel);
        if (client == null)
        {
            client = new Client
            {
                FirstName = clientTripDTO.FirstName,
                LastName = clientTripDTO.LastName,
                Email = clientTripDTO.Email,
                Telephone = clientTripDTO.Telephone,
                Pesel = clientTripDTO.Pesel
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        var trip = await _context.Trips.FindAsync(tripId);
        if (trip == null)
        {
            return BadRequest("Trip not found.");
        }

        var existingClientTrip = await _context.ClientTrips
            .SingleOrDefaultAsync(ct => ct.ClientId == client.Id && ct.TripId == tripId);
        if (existingClientTrip != null)
        {
            return BadRequest("Client is already registered for this trip.");
        }

        var clientTrip = new ClientTrip
        {
            ClientId = client.Id,
            TripId = tripId,
            PaymentDate = clientTripDTO.PaymentDate,
            RegisteredAt = DateTime.Now
        };
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();

        return Ok();
    }
}