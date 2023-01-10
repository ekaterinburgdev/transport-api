using Newtonsoft.Json;
using Transport.Models;
using Transport.Models.Ettu;

namespace Transport.Clients;

public class EttuClient
{
    private const string BaseUrl = "http://map.ettu.ru/api/v2/{0}/boards/?apiKey=111";

    private HttpClient httpClient;

    public EttuClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Models.Transport[]> GetAllTransportsAsync()
    {
        var trams = await GetTransportsAsync(TransportType.Tram) ?? Array.Empty<Models.Transport>();
        var trolls = await GetTransportsAsync(TransportType.Troll) ?? Array.Empty<Models.Transport>();
        return trams.Concat(trolls).ToArray();
    }
    
    public async Task<Models.Transport[]?> GetTransportsAsync(TransportType type)
    {
        var a = type.ToString();
        var response = await httpClient
            .GetAsync(string.Format(BaseUrl, type.ToString().ToLower()));
        var responseString = await response.Content.ReadAsStringAsync();
        var ettuResponse = JsonConvert.DeserializeObject<EttuResponse>(responseString);
        return ettuResponse?.Vehicles.Select(vehicle => new Models.Transport
        {
            Type = type,
            BoardId = string.IsNullOrEmpty(vehicle.BOARD_ID) ? 0 : int.Parse(vehicle.BOARD_ID),
            Course = string.IsNullOrEmpty(vehicle.COURSE) ? 0 : int.Parse(vehicle.COURSE),
            Position = (LAT: 0, LON: 0),
            Route = string.IsNullOrEmpty(vehicle.ON_ROUTE) ? 0 : int.Parse(vehicle.ON_ROUTE),
            Velocity = string.IsNullOrEmpty(vehicle.VELOCITY) ? 0 : int.Parse(vehicle.VELOCITY)

        }).ToArray();
    }
}