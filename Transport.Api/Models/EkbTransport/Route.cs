using Transport.Models.EkbTransport.Stop;

namespace Transport.Models.EkbTransport;

public class Route
{
    public string mr_id { get; set; }
    public string mr_num { get; set; }
    public string mr_title { get; set; }
    public string mr_title_en { get; set; }
    public string tt_id { get; set; }
    public Race[] races { get; set; }
    public Park[] parks { get; set; }
}

public class Race
{
    public string rl_id { get; set; }
    public string mr_id { get; set; }
    public string rl_racetype { get; set; }
    public string rl_firststation_id { get; set; }
    public string rl_laststation_id { get; set; }
    public string rl_firststation { get; set; }
    public string rl_firststation_en { get; set; }
    public string rl_laststation { get; set; }
    public string rl_laststation_en { get; set; }
    public StopInfo[] stopList { get; set; }
    public Coordinates[] coordList { get; set; }
}

public class Coordinates
{
    public string rd_lat { get; set; }
    public string rd_long { get; set; }
}

public class Park
{
    public string pk_id { get; set; }
    public string pk_title { get; set; }
    public string pk_title_en { get; set; }
}