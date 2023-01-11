namespace Transport.Models.Api;

public class JsonRpcResult<T>
{
    public T Result { get; set; }
    public JsonRpcError Error { get; set; }
}

public class JsonRpcError
{
    public int Code { get; set; }
    public string Message { get; set; }
}