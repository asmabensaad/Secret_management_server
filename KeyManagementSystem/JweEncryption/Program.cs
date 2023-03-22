



using JweEncryption;

class Program
{
 /*   static void Main(string[] args)
    {
        Console.WriteLine("enter the data to encrypt");
        string datatoencrypt = Console.ReadLine();
        var encryptedText = SymmetricEncryptionDecryptionManager.Encrypt(datatoencrypt, "azerty56478demo1");
        Console.WriteLine(encryptedText);
        Console.WriteLine(("do you want to see the encrypted data in plaintext press enter key"));
        ConsoleKeyInfo cki;
        while (true)
        {
            cki = Console.ReadKey();
            if (cki.Key == ConsoleKey.Escape)
                break;
            else
            {
                var decryptedText = SymmetricEncryptionDecryptionManager.Decrypt(encryptedText, "azerty56478demo1");
                Console.WriteLine(decryptedText);
            }
        }
        
    }*/
}




























// using System.Security.Cryptography;
// using System.Text;
// using Jose;
// using Microsoft.IdentityModel.JsonWebTokens;
// using Microsoft.IdentityModel.Tokens;
//
// var handler = new JsonWebTokenHandler();
//
// //string msg="hello word";
// byte[] key;
//
//
//
//
// var encryptionKey = RSA.Create(3072);
//
// static void Main(string[] args, out byte[] key)
// {
//
//
//     key = tripleDes.Key;
//     Console.WriteLine((string.Join(",",key)));
//     
// }





// string JwtTokenEncryption(string jsonData, string sharedSecret, string apiKey)
// 
//     using (SHA256 sha256Hash = SHA256.Create())
//     {
//         var utc0 = new DateTime(1970, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc);
//         var iat = (int)DateTime.Now.Subtract(utc0).TotalSeconds;
//
//         var digest = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(sharedSecret));
//
//         var extraHeaders = new Dictionary<string, object>
//         {
//             { "kid", apiKey },
//             { "typ", "JOSE" },
//             { "channelSecurityContext", "SHARED_SECRET" },
//             { "iat", iat },
//         };
//
//         string result = Jose.JWT.Encode(jsonData, digest, JweAlgorithm.A256GCMKW, JweEncryption.A256GCM,
//             extraHeaders: extraHeaders);
//
//         return result;
//     }
//     
// }


