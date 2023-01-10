namespace Transport.Models;

public class Transport
{
    public TransportType Type { get; set; }
    public (double LAT, double LON) Position { get; set; }
    public int Route { get; set; }
    public int BoardId { get; set; }
    public int Velocity { get; set; }
    public int Course { get; set; }
}