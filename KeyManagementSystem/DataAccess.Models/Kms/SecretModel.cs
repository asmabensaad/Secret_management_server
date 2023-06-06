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
    [JsonProperty("key")]
    [Required]
    [MaxLength(255)]
    [MinLength(3)]
    public string Key { get; set; }

    /// <summary>
    /// Dict
    /// </summary>
    [JsonProperty("Dictionary")]
    [Required(ErrorMessage = "")]
    [MaxLength(255)]
    [MinLength(3)]
    public Dictionary<string, object> Secretvalue { get; set; }
 



}