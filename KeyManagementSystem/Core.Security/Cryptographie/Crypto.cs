using System.Security.Cryptography;
using System.Text;
using Jose;


namespace Core.Security.Cryptographie;

//TODO: Format code

/// <inheritdoc cref="ICrypto"/>
public class Crypto : ICrypto
{
    public enum Algorithm
    {
        //HMAC signatures with HS256, HS384 and HS512.
        Hs256 = 0,
        Hs384 = 1,
        Hs512 = 2
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
        var i = (int)alg;
        var cipherText = i switch
        {
            0 => JWT.EncodeBytes(data, key, JweAlgorithm.PBES2_HS256_A128KW, JweEncryption.A128CBC_HS256),
            1 => JWT.EncodeBytes(data, key, JweAlgorithm.PBES2_HS384_A192KW, JweEncryption.A192CBC_HS384),
            _ => JWT.EncodeBytes(data, key, JweAlgorithm.PBES2_HS512_A256KW, JweEncryption.A256CBC_HS512)
        };

        var encryptedData = Encoding.UTF8.GetBytes(cipherText);
        return encryptedData;
    }

    /// <inheritdoc cref="ICrypto.Decrypt"/>
    public byte[] Decrypt(byte[] cipherText, Algorithm alg, byte[] keyBytes)
    {
        var key = Encoding.UTF8.GetString(keyBytes);
        var i = (int)alg;
        var data = i switch
        {
            0 => JWT.DecodeBytes(Encoding.UTF8.GetString(cipherText), key, JweAlgorithm.PBES2_HS256_A128KW,
                JweEncryption.A128CBC_HS256),
            1 => JWT.DecodeBytes(Encoding.UTF8.GetString(cipherText), key, JweAlgorithm.PBES2_HS384_A192KW,
                JweEncryption.A192CBC_HS384),
            _ => JWT.DecodeBytes(Encoding.UTF8.GetString(cipherText), key, JweAlgorithm.PBES2_HS512_A256KW,
                JweEncryption.A256CBC_HS512)
        };

        return data;
    }
}