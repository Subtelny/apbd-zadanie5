namespace s24422_zad5.Models;

public class Trip
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<Country> Countries { get; set; }
    public List<ClientTrip> ClientTrips { get; set; }
}