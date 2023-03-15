using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BouncyCastleApp
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            // First load a Certificate, filename/path and certificate password
            //var Cert = ReadCertFromFile("./example.crt", "1234");
            var cert = new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "example.crt"), "1234");
            // var cert = new X509Certificate2(@".\example.crt"); //  Select a binary file

            var key = new X509SecurityKey(cert);

            var jwk = JsonWebKeyConverter.ConvertFromX509SecurityKey(key, true);

            
            
            

            IList<JsonWebKey> jwksList = new List<JsonWebKey>
            {
                jwk
            };

            Dictionary<string, IList<JsonWebKey>> jwksDict = new()
            {
                { "keys", jwksList }
            };

            var jwksStr = JsonConvert.SerializeObject(jwksDict, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            });

            // string jwksStr =SerializeToJson(jwksDict);
            Console.WriteLine(jwksStr);

            // JsonWebTokenHandler tokenHandler = new();
        }
    }
}