namespace Transport.Models.EkbTransport;

public class Timetable
{
    public string srv_id { get; set; }
    public string rv_id { get; set; }
    public string rv_dow { get; set; }
    public string rv_season { get; set; }
    public string rv_startdate { get; set; }
    public string rv_enddate { get; set; }
    public string rv_enddateexists { get; set; }
    public string rv_num { get; set; }
    public TimetableStop[] stopList { get; set; }
    public object races { get; set; }
}

public class TimetableStop
{
    public string rc_orderby { get; set; }
    public string st_id { get; set; }
    public string st_title { get; set; }
    public string st_title_en { get; set; }
    public string rc_kkp { get; set; }
    public HourInfo[] hours { get; set; }
}

public class HourInfo
{
    public int Hour { get; set; }
    public Minute[] Minutes { get; set; }
}

public class Minute
{
    public string minute { get; set; }
    public string rl_racetype { get; set; }
}

