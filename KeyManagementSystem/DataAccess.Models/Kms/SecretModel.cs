using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DataAccess.Models.Kms;

/// <summary>
/// Secret model
/// </summary>
public class SecretModel
{
    /// <summary>
    /// Key 
    /// </summary>
    [JsonProperty("secret")]
    [Required]
    [MaxLength(255)]
    [MinLength(1)]
    public string Secret { get; set; }

    /// <summary>
    /// Secretvalue
    /// </summary>
    [JsonProperty("secretValue")]
    [Required(ErrorMessage = "")]
    [MaxLength(255)]
    [MinLength(1)]
    public Dictionary<string, object> SecretValue { get; set; }
}