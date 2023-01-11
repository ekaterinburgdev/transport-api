namespace Transport.Models.EkbTransport;

public class TransportInfo
{
    public string Tt_Id { get; set; }
    public string Tt_Title { get; set; }
    public string Tt_Title_En { get; set; }
    public TransportRoute[] Routes { get; set; }
}

public class TransportRoute
{
    public string Mr_Id { get; set; }
    public string Mr_Num { get; set; }
    public string Mr_Title { get; set; }
    public string Mr_Title_En { get; set; }
}