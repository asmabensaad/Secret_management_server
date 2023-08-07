using Newtonsoft.Json;

namespace Core.Security.Vault;

public class VaultTokenResponse
{
    [JsonProperty("request_id")]
    public string RequestId { get; set; }

    [JsonProperty("lease_id")]
    public string LeaseId { get; set; }

    [JsonProperty("renewable")]
    public bool Renewable { get; set; }

    [JsonProperty("lease_duration")]
    public int LeaseDuration { get; set; }

    [JsonProperty("data")]
    public object Data { get; set; }

    [JsonProperty("wrap_info")]
    public object WrapInfo { get; set; }

    [JsonProperty("warnings")]
    public object Warnings { get; set; }

    [JsonProperty("auth")]
    public VaultAuth Auth { get; set; }
}