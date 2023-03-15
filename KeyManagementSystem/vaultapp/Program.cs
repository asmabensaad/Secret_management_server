using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vaultapp
{
    
    class Program
    {
        // The since CS 7.1 TYPE can be Task instead of void
        static async Task Main(string[] args)
        {
            // Creating an instance of the HTTP Client
            HttpClient httpClient = HttpClientFactory.Create();
            //Setting the URL to our HashiCorp Secret
            string url = "http://127.0.0.1:8200/v1/secret/data/person";
            string url2= "http://127.0.0.1:8200/v1/secret/data/admin";
            // Setting the Token Header and the Root Token
            httpClient.DefaultRequestHeaders.Add("X-Vault-Token", "myroot");
            // Setting the Content-type to application/json
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Making the HTTP Get call to consult our Secret
            JObject json= JObject.Parse(await httpClient.GetStringAsync(url));
            JObject json2= JObject.Parse(await httpClient.GetStringAsync(url2));
            // Printing the response
            Console.WriteLine(json);
            // Storing the key-value pairs of our secret from the response
            JToken secrets = json["data"]["data"];
            JToken secrets1 = json2["data"]["data"];
            // Validating the previous statement is true
            Console.WriteLine("\n" + secrets);
            Console.WriteLine("\n" + secrets1);
            // Storing our key-value pairs to a Dictionary for future data manipulation
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(secrets.ToString());
            Dictionary<string, string> values2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(secrets1.ToString());
            // Looping through our key-value pairs
            foreach (var item in values)
            {
                // Printing our key-value pairs
                Console.WriteLine($"Key: {item.Key} Value: {item.Value}");
            }
            foreach (var item in values2)
            {
                // Printing our key-value pairs
                Console.WriteLine($"Key: {item.Key} Value: {item.Value}");
            }
            
            
            
        }
    }
}