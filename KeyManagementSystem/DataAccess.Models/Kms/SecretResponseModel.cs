using Newtonsoft.Json;

namespace DataAccess.Models.Kms;

/// <summary>
///  Secret response model
/// </summary>
public class SecretResponseModel : SecretModel
{
    /// <summary>
    /// Version
    /// </summary>
    [JsonProperty]
    public string Version { get; set; }
}