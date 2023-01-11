namespace Transport.Models.EkbTransport;

public class TransportInfo
{
    public string Id { get; set; }
    public string Title { get; set; }
    public TransportRoute[] Routes { get; set; }
}

public class TransportRoute
{
    public string Id { get; set; }
    public string Num { get; set; }
    public string Title { get; set; }
}