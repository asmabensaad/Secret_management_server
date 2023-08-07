using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DataAccess.Models.Api;

/// <summary>
/// API errors
/// </summary>
public enum ApiError
{
    /// <summary>
    /// Invalid data error code
    /// </summary>
    [EnumMember(Value = "INVALID_DATA")] [JsonProperty("INVALID_DATA")]
    InvalidData,
    
    /// <summary>
    /// Secret not found error code
    /// </summary>
    [EnumMember(Value = "SECRET_NOT_FOUND")] [JsonProperty("SECRET_NOT_FOUND")]
    SecretNotFound,

    /// <summary>
    /// Server error code
    /// </summary>
    [EnumMember(Value = "SERVER_ERROR")] [JsonProperty("SERVER_ERROR")]
    ServerError,
}