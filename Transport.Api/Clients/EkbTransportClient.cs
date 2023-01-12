using System.Globalization;
using System.Text;
using System.Text.Json;
using Transport.Extensions;
using Transport.Models.Api;
using Transport.Models.EkbTransport;
using Transport.Models.EkbTransport.Stop;
using Transport.Models.EkbTransport.Unit;

namespace Transport.Clients;

public class EkbTransportClient
{
    private const string BaseUrl = "http://xn--80axnakf7a.xn--80acgfbsl1azdqr.xn--p1ai/api/rpc.php";

    private string SessionId { get; set; }
    private int TaskId { get; set; }
    
    public async Task<Timetable> GetRaspisanie(string mrId, DateOnly date, string raceType, string kpp, string stopId)
    {
        return await GetContentAsync<Timetable>("getRaspisanie", new Dictionary<string, string>
        {
            ["mr_id"] = mrId,
            ["data"] = date.ToString("yyyy-MM-dd"),
            ["rl_racetype"] = raceType,
            ["rc_kkp"] = kpp,
            ["st_id"] = stopId
        }).ConfigureAwait(false); 
    }
    
    public async Task<RaceTree[]> GetRaceTree(string mrId, DateOnly date)
    {
        return await GetContentAsync<RaceTree[]>("getRaceTree", new Dictionary<string, string>
        {
            ["mr_id"] = mrId,
            ["data"] = date.ToString("yyyy-MM-dd")
        }).ConfigureAwait(false); 
    }
    
    public async Task<UnitArriveInfo[]> GetUnitArrive(string unitId)
    {
        return await GetContentAsync<UnitArriveInfo[]>("getUnitArrive", new Dictionary<string, string>
        {
            ["u_id"] = unitId,
        }).ConfigureAwait(false); 
    }
    
    public async Task<StopArriveInfo[]> GetStopArrive(int stopId)
    {
        return await GetContentAsync<StopArriveInfo[]>("getStopArrive", new Dictionary<string, string>
        {
            ["st_id"] = stopId.ToString(),
        }).ConfigureAwait(false); 
    }
    
    public async Task<UnitInfo[]> GetUnitsInRectangle(double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
    {
        return await GetContentAsync<UnitInfo[]>("getUnitsInRect", new Dictionary<string, string>
        {
            ["minlat"] = minLatitude.ToString(CultureInfo.InvariantCulture),
            ["maxlat"] = maxLatitude.ToString(CultureInfo.InvariantCulture),
            ["minlong"] = minLongitude.ToString(CultureInfo.InvariantCulture),
            ["maxlong"] = maxLongitude.ToString(CultureInfo.InvariantCulture)
        }).ConfigureAwait(false); 
    }

    public async Task<StopInfo[]> GetStopsInRectangle(double minLatitude, double maxLatitude, double minLongitude, double maxLongitude)
    {
        return await GetContentAsync<StopInfo[]>("getStopsInRect", new Dictionary<string, string>
        {
            ["minlat"] = minLatitude.ToString(CultureInfo.InvariantCulture),
            ["maxlat"] = maxLatitude.ToString(CultureInfo.InvariantCulture),
            ["minlong"] = minLongitude.ToString(CultureInfo.InvariantCulture),
            ["maxlong"] = maxLongitude.ToString(CultureInfo.InvariantCulture)
        }).ConfigureAwait(false); 
    }

    public async Task<StopInfo[]> GetStopsByName(string stopName)
    {
        return await GetContentAsync<StopInfo[]>("getStopsByName", new Dictionary<string, string>
        {
            ["str"] = stopName,
            ["ok_id"] = ""
        }).ConfigureAwait(false);
    }

    public async Task<RegionCenter[]> GetRegionCenter()
    {
        return await GetContentAsync<RegionCenter[]>("getRegionCenter", new Dictionary<string, string>
        {
            ["ok_id"] = ""
        }).ConfigureAwait(false);
    }
    
    public async Task<Okato[]> GetOkatoList()
    {
        return await GetContentAsync<Okato[]>("getOkatoList");
    }
    
    public async Task<TransportInfo[]> GetTransTypes()
    {
        return await GetContentAsync<TransportInfo[]>("getTransTypeTree", new Dictionary<string, string>
        {
            ["ok_id"] = ""
        });
    }

    private async Task<T> GetContentAsync<T>(string action, Dictionary<string, string>? parameters = null)
    {
        var responseString = await GetResponseAsync(action, parameters).ConfigureAwait(false);
        var result = JsonSerializer.Deserialize<JsonRpcResult<T>>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            MaxDepth = 10
        });
        result.EnsureSuccess();
        return result.Result;
    }

    private async Task<string> GetResponseAsync(string action, Dictionary<string, string>? parameters = null)
    {
        parameters ??= new Dictionary<string, string>();
        using var httpClient = new HttpClient();
        var request = await GetRequest(action, parameters);
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        return responseString;
    }

    private async Task<HttpRequestMessage> GetRequest(string action, Dictionary<string, string> parameters)
    {
        if (string.IsNullOrEmpty(SessionId))
            await InitializeSessionId();
        var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl);
        var json = JsonSerializer.Serialize(new
        {
            id = TaskId++,
            method = action,
            jsonrpc = "2.0",
            @params = new Dictionary<string, string>(parameters)
            {
                ["sid"] = SessionId,
            }
        });
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        return request;
    }

    private async Task InitializeSessionId()
    {
        var message = new HttpRequestMessage(HttpMethod.Post, BaseUrl);
        message.Content = new StringContent(JsonSerializer.Serialize(new
        {
            jsonrpc = "2.0",
            method = "startSession",
            @params = new
            {
            },
            id = TaskId++
        }), Encoding.UTF8, "application/json");
        using var httpClient = new HttpClient();
        var response = await httpClient.SendAsync(message);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var responseJson = JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);
        var resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseJson["result"].ToString());
        SessionId = resultDictionary["sid"].ToString();
    }
}