using System.Reflection.Metadata.Ecma335;

namespace Transport.Models.Ettu;

public class EttuResponse
{
    public (string Code, string Msg) Error { get; set; }
    public EttuVehicle[] Vehicles { get; set; }
}