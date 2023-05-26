using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DataAccess.Models.Api;

/// <summary>
/// API errors
/// </summary>
public enum ApiError
{
    /// <summary>
    /// Invalid data code
    /// </summary>
    [EnumMember(Value = "INVALID_DATA")] [JsonProperty("INVALID_DATA")]
    InvalidData,

    /// <summary>
    /// Server error code
    /// </summary>
    [EnumMember(Value = "SERVER_ERROR")] [JsonProperty("SERVER_ERROR")]
    ServerError,
}