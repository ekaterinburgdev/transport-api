namespace Transport.Models.EkbTransport;

public class RaceTree
{
    public string Rl_Id { get; set; }
    public string Mr_Id { get; set; }
    public string Rl_Racetype { get; set; }
    public string Rl_Firststation_Id { get; set; }
    public string Rl_Laststation_Id { get; set; }
    public string Rl_Firststation { get; set; }
    public string Rl_Firststation_En { get; set; }
    public string Rl_Laststation { get; set; }
    public string Rl_Laststation_En { get; set; }
    public RaceStop[] StopList { get; set; }
}

public class RaceStop
{
    public int Rc_Orderby { get; set; }
    public object St_Id { get; set; }
    public string St_Title { get; set; }
    public string St_Title_En { get; set; }
    public string Rc_Kkp { get; set; }
}