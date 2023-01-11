using Transport.Models.Api;

namespace Transport.Extensions;

public static class JsonRpcResultExtensions
{
    public static void EnsureSuccess<T>(this JsonRpcResult<T> result)
    {
        if (result.Error != null)
            throw new Exception(result.Error.Message);
    }
}