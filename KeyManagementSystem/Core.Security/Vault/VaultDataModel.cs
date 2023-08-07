using Newtonsoft.Json;

namespace Core.Security.Vault;

public class VaultDataModel
{
    /// <summary>
    /// Ilist key
    /// </summary>
    [JsonProperty("keys")] public IList<string> Keys { get; set; }
}