
using System.Security.Cryptography;
using System.Text;
using Jose;


namespace Core.Security.Cryptographie;

//TODO: Add an interface, and document the code.
//TODO: Remove any console log
//TODO: Follow naming convention
//TODO: Remove commented code
public class Crypto : ICrypto
{
    

    public enum Algorithm
    {
        //HMAC signatures with HS256, HS384 and HS512.
        Hs256 = 0,
        Hs384 = 1,
        Hs512 = 2
    }

/// <summary>
/// Generate KeyByte
/// </summary>
/// <returns></returns>

    [Obsolete("Obsolete")]
    private static byte[] GenerateEncryptionKey()
    {
        const int keySizeInByte = 32;
        var keyBytes = new byte[keySizeInByte];
        using var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(keyBytes);
        return keyBytes;


    }

    [Obsolete("Obsolete")] internal byte[] KeyBytes = GenerateEncryptionKey();
    /// <summary>
    /// dataEncryption
    /// </summary>
    /// <param name="data"></param>
    /// <param name="alg"></param>
    /// <param name="keyBytes"></param>
    /// <returns></returns>

    public byte[] Encrypt(byte[] data, Algorithm alg, byte[] keyBytes)
    {
        var key = Encoding.UTF8.GetString(keyBytes);
        string cipherText ;
        var i = (int)alg;
        switch (i)
        {
            case 0:
            {
                cipherText = JWT.EncodeBytes(data, key, JweAlgorithm.PBES2_HS256_A128KW,
                    JweEncryption.A128CBC_HS256);

                break;
            }
            case 1:
            {
                cipherText = JWT.EncodeBytes(data, key, JweAlgorithm.PBES2_HS384_A192KW,
                    JweEncryption.A192CBC_HS384);

                break;
            }
            default:
            {
                cipherText = JWT.EncodeBytes(data, key, JweAlgorithm.PBES2_HS512_A256KW,
                    JweEncryption.A256CBC_HS512);
                break;
            }


        }

        var encryptedData = Encoding.UTF8.GetBytes(cipherText);
        return encryptedData;

    }
    /// <summary>
    /// dataDecryption
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="alg"></param>
    /// <param name="keyBytes"></param>
    /// <returns></returns>

    public byte[] Decrypt(byte[] cipherText, Algorithm alg, byte[] keyBytes)
    {
        var key = Encoding.UTF8.GetString(keyBytes);
        byte[] data;
        var i = (int)alg;
        switch (i)
        {
            case 0:
            {
                data = JWT.DecodeBytes(Encoding.UTF8.GetString(cipherText), key, JweAlgorithm.PBES2_HS256_A128KW,
                    JweEncryption.A128CBC_HS256);
               
                break;
            }
            case 1:
            {
                data = JWT.DecodeBytes(Encoding.UTF8.GetString(cipherText), key,
                    JweAlgorithm.PBES2_HS384_A192KW, JweEncryption.A192CBC_HS384);
                break;

            }
            default:
            {
                data = JWT.DecodeBytes(Encoding.UTF8.GetString(cipherText), key,
                    JweAlgorithm.PBES2_HS512_A256KW, JweEncryption.A256CBC_HS512);
                break;

            }
            
        }
        return data;
    }
}

