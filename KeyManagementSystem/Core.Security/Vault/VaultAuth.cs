using Newtonsoft.Json;

namespace Core.Security.Vault;

public class VaultAuth
{
    /// <summary>
    /// ClientToken
    /// </summary>
    [JsonProperty("client_token")]
    public string ClientToken { get; set; }

    /// <summary>
    /// Accessor
    /// </summary>
    [JsonProperty("accessor")]
    public string Accessor { get; set; }

    /// <summary>
    /// Policies
    /// </summary>
    [JsonProperty("policies")]
    public IList<string> Policies { get; set; }

    /// <summary>
    /// TokenPolicies
    /// </summary>
    [JsonProperty("token_policies")]
    public IList<string> TokenPolicies { get; set; }

    /// <summary>
    /// Metadata
    /// </summary>
    [JsonProperty("metadata")]
    public VaultMetadata Metadata { get; set; }

    /// <summary>
    /// LeaseDuration
    /// </summary>
    [JsonProperty("lease_duration")]
    public int LeaseDuration { get; set; }

    /// <summary>
    /// Renewable
    /// </summary>
    [JsonProperty("renewable")]
    public bool Renewable { get; set; }

    /// <summary>
    /// EntityId
    /// </summary>
    [JsonProperty("entity_id")]
    public string EntityId { get; set; }

    /// <summary>
    /// TokenType
    /// </summary>
    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    /// <summary>
    /// Orphan
    /// </summary>
    [JsonProperty("orphan")]
    public bool Orphan { get; set; }

    /// <summary>
    /// MfaRequirement
    /// </summary>
    [JsonProperty("mfa_requirement")]
    public object MfaRequirement { get; set; }

    /// <summary>
    /// NumUses
    /// </summary>
    [JsonProperty("num_uses")]
    public int NumUses { get; set; }
}