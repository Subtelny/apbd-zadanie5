namespace s24422_zad5.Models;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TripId { get; set; }
    public Trip Trip { get; set; }
}