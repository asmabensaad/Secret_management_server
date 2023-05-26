using System.Transactions;
using Newtonsoft.Json;

namespace DataAccess.Models.Kms;
/// <summary>
///  SecretResponseModel
/// </summary>
public class SecretResponseModel: SecretModel
{ /// <summary>
 /// Version
 /// </summary>
    [JsonProperty]
    public string Version { get; set; }

}