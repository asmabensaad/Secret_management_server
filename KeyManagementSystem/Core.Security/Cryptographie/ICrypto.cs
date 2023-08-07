using Jose;

namespace Core.Security.Cryptographie;

//TODO: Document the code.
/// <summary>
/// interface ICrypto
/// </summary>
public interface ICrypto
{
    /// <summary>
    /// Encryption Function
    /// </summary>
    /// <param name="data"></param>
    /// <param name="alg"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public byte[] Encrypt(byte[] data, Crypto.Algorithm alg, byte[] key);

    /// <summary>
    /// Decryption Function 
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="alg"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public byte[] Decrypt(byte[] cipherText, Crypto.Algorithm alg, byte[] key);

    /// <summary>
    /// Generate an Encryption key
    /// </summary>
    /// <returns></returns>
    public byte[] GenerateEncryptionKey();
}