
using Jose;
namespace Core.Security.Cryptographie;

public interface ICrypto
{
    public byte[] Encrypt(byte[] data, Crypto.Algorithm alg,byte[] key);

    public byte[] Decrypt(byte[]  cipherText ,Crypto.Algorithm alg, byte[] key);
    
}