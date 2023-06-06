using System.Security.Cryptography.X509Certificates;
using System.Text;

using Jose;

namespace Core.Security.Cryptographie;

public class Crypto
{
    private static readonly KmsVaultClient Client = new KmsVaultClient();

    public enum algorithm
    {
        ECDH,
        HS256,
        AES
    }

    /// <summary>
    /// EncryptData
    /// </summary>
    /// <returns></returns>
    public static async Task<string> EncryptData(algorithm alg)
    {
        Client.SetVaultAddress("http://127.0.0.1:8200").SetUserName("admin").SetPassword("admin");

        string payload = await Client.GetSecretAsyn(key: "first", path: "/kms");

        int i = (int)alg;

        switch (i)
        {
            case 0:
                {
                    //ECDH-ES with AES (advanced Encryption symetric)

                    var publicKey = new Jwk(
                        crv: "P-256",
                        x: "BHId3zoDv6pDgOUh8rKdloUZ0YumRTcaVDCppUPoYgk",
                        y: "g3QIDhaWEksYtZ9OWjNHn9a6-i_P9o5_NrdISP0VWDU"
                    );

                    string token = JWT.Encode(payload, publicKey, JweAlgorithm.ECDH_ES, JweEncryption.A256GCM);
                    Console.WriteLine("token =" + token);


                    break;
                }
            case 1:
                {
                    //HS256

                    string token = JWT.Encode(payload, "top secret", JweAlgorithm.PBES2_HS256_A128KW,
                        JweEncryption.A256CBC_HS512);
                    Console.WriteLine("token =" + token);


                    break;
                }
            default:
                {
                    //AES
                    var secretKey =
                        Encoding.UTF8.GetBytes("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");

                    string token = Jose.JWT.Encode(payload, secretKey, JweAlgorithm.A256GCMKW,
                        JweEncryption.A256CBC_HS512);

                    Console.WriteLine("token =" + token);


                    break;
                }
        }

        return default;
    }


    /// <summary>
    /// Decryptdata
    /// </summary>
    /// <returns></returns>
    public static async Task<string> Decryptdata(algorithm alg)
    {
        // var privatekey = new X509Certificate2(Path.GetFullPath("/home/asma/openssl/domain.pfx"), "1234").GetECDsaPrivateKey();
        // var dataToDecrypt = JWT.Decode(encryptdata, privatekey, JwsAlgorithm.ES256);

        string encryptdata = await EncryptData(alg);


        int i = (int)alg;

        switch (i)
        {
            case 0:
                {
                    //ECDH-ES with AES (advanced Encryption symetric)
                    var publicKey = new Jwk(
                        crv: "P-256",
                        x: "BHId3zoDv6pDgOUh8rKdloUZ0YumRTcaVDCppUPoYgk",
                        y: "g3QIDhaWEksYtZ9OWjNHn9a6-i_P9o5_NrdISP0VWDU"
                    );

                    string decryptedData = JWT.Decode(encryptdata, publicKey);
                    Console.WriteLine("decrpteddata=" + decryptedData);
                    break;

                }
            case 1:
                {
                    //HS256

                    string decryptedData = JWT.Decode(encryptdata, "top secret");
                    Console.WriteLine("decryptedData=" + decryptedData);
                    break;
                }
            default:
                {
                    var secretKey =
                        Encoding.UTF8.GetBytes("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    string decryptedData = JWT.Decode(encryptdata, secretKey);
                    Console.WriteLine("decryptedData=" + decryptedData);
                    break;

                }

        }

        return default;
    }
   
}