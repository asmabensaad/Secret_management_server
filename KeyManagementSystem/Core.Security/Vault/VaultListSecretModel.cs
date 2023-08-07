using Newtonsoft.Json;

namespace Core.Security.Vault;

public class VaultListSecretModel
{
    /// <summary>
    /// RequestId
    /// </summary>
    [JsonProperty("request_id")]
    public string RequestId { get; set; }

    /// <summary>
    /// LeaseId
    /// </summary>
    [JsonProperty("lease_id")]
    public string LeaseId { get; set; }

    /// <summary>
    /// Renewable
    /// </summary>
    [JsonProperty("renewable")]
    public bool Renewable { get; set; }

    /// <summary>
    /// LeaseDuration
    /// </summary>
    [JsonProperty("lease_duration")]
    public int LeaseDuration { get; set; }

    /// <summary>
    /// VaultData
    /// </summary>
    [JsonProperty("data")]
    public VaultDataModel VaultData { get; set; }

    /// <summary>
    /// WrapInfo
    /// </summary>
    [JsonProperty("wrap_info")]
    public object WrapInfo { get; set; }

    /// <summary>
    /// Warnings
    /// </summary>
    [JsonProperty("warnings")]
    public object Warnings { get; set; }

    /// <summary>
    /// Auth
    /// </summary>
    [JsonProperty("auth")]
    public object Auth { get; set; }
}