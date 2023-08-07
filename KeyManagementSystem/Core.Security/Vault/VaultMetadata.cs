using Newtonsoft.Json;

namespace Core.Security.Vault;

public class VaultMetadata
{
    /// <summary>
    /// Username
    /// </summary>
    [JsonProperty("username")]
    public string Username { get; set; }
}