using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using s24422_zad5.Dtos;
using s24422_zad5.Models;

namespace s24422_zad5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController(TripContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TripDTO>>> GetTrips()
    {
        var trips = await context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Include(t => t.Countries)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.Client)
            .ToListAsync();

        return trips.Select(t => new TripDTO
        {
            Name = t.Name,
            Description = t.Description,
            DateFrom = t.DateFrom,
            DateTo = t.DateTo,
            MaxPeople = t.MaxPeople,
            Countries = t.Countries.Select(c => new CountryDTO { Name = c.Name }).ToList(),
            Clients = t.ClientTrips.Select(ct => new ClientDTO
                { FirstName = ct.Client.FirstName, LastName = ct.Client.LastName }).ToList()
        }).ToList();
    }
}