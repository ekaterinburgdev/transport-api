using Transport.Extensions;
using Transport.Models.Api;
using Transport.Models.EkbTransport;

namespace Transport.Clients;

using System.Text;
using System.Text.Json;

public class EkbTransportClient
{
    private const string RequestUri = "http://xn--80axnakf7a.xn--80acgfbsl1azdqr.xn--p1ai/api/rpc.php";

    private string SessionId { get; set; }
    private int TaskId { get; set; }

    public async Task<TransportInfo[]> GetTransTypes()
    {
        using var httpClient = new HttpClient();
        var request = await GetRequest("getTransTypeTree", new Dictionary<string, string>
        {
            ["ok_id"] = ""
        });
        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonRpcResult<TransportInfo[]>>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });
        result.EnsureSuccess();
        return result.Result;
    }

    private async Task<HttpRequestMessage> GetRequest(string action, Dictionary<string, string> parameters)
    {
        if (string.IsNullOrEmpty(SessionId))
            await InitializeSessionId();
        var request = new HttpRequestMessage(HttpMethod.Post, RequestUri);
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
        var message = new HttpRequestMessage(HttpMethod.Post, RequestUri);
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