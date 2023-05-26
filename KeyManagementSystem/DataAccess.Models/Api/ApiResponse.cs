using Newtonsoft.Json;

namespace DataAccess.Models.Api;

/// <summary>
/// API response
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Response data
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; }

    /// <summary>
    /// API error
    /// </summary>
    [JsonProperty("error")]
    public ApiError? Error { get; set; }
}