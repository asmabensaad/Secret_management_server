using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Jose;


namespace Core.Security.Cryptographie;

/// <inheritdoc cref="ICrypto"/>
public class Crypto : ICrypto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Algorithm
    {
        //HMAC signatures with HS256, HS384 and HS512.
        Hs256,
        Hs384,
        Hs512
    }

    /// <inheritdoc cref="ICrypto.GenerateEncryptionKey"/>
    [Obsolete("Obsolete")]
    public byte[] GenerateEncryptionKey()
    {
        const int keySizeInByte = 32;
        var keyBytes = new byte[keySizeInByte];
        using var rng = new RNGCryptoServiceProvider();
        rng.GetBytes(keyBytes);
        return keyBytes;
    }

    /// <inheritdoc cref="ICrypto.Encrypt"/>
    public byte[] Encrypt(byte[] data, Algorithm alg, byte[] keyBytes)
    {
        var key = Encoding.UTF8.GetString(keyBytes);
        var cipherText = alg switch
        {
            Algorithm.Hs256 => JWT.EncodeBytes(data, key, JweAlgorithm.PBES2_HS256_A128KW, JweEncryption.A128CBC_HS256),
            Algorithm.Hs384 => JWT.EncodeBytes(data, key, JweAlgorithm.PBES2_HS384_A192KW, JweEncryption.A192CBC_HS384),
            _ => JWT.EncodeBytes(data, key, JweAlgorithm.PBES2_HS512_A256KW, JweEncryption.A256CBC_HS512)
        };

        var encryptedData = Encoding.UTF8.GetBytes(cipherText);
        return encryptedData;
    }

    /// <inheritdoc cref="ICrypto.Decrypt"/>
    public byte[] Decrypt(byte[] cipherText, Algorithm alg, byte[] keyBytes)
    {
        var key = Encoding.UTF8.GetString(keyBytes);
        var data = alg switch
        {
            Algorithm.Hs256 => JWT.DecodeBytes(Encoding.UTF8.GetString(cipherText), key,
                JweAlgorithm.PBES2_HS256_A128KW,
                JweEncryption.A128CBC_HS256),
            Algorithm.Hs384 => JWT.DecodeBytes(Encoding.UTF8.GetString(cipherText), key,
                JweAlgorithm.PBES2_HS384_A192KW,
                JweEncryption.A192CBC_HS384),
            _ => JWT.DecodeBytes(Encoding.UTF8.GetString(cipherText), key, JweAlgorithm.PBES2_HS512_A256KW,
                JweEncryption.A256CBC_HS512)
        };

        return data;
    }
}