using System.Text;
using Core.Security.Cryptographie;

namespace UnitTests;

[TestClass]
public class CryptoTest
{
    private readonly ICrypto _crypto = new Crypto();


    [TestMethod]
    public void TestEncryptionAndDecryption()
    {
        //Arrange
        var dataEncrypt = Encoding.UTF8.GetBytes("sensitive data to encrypt");
        var encryptionKey = new byte[32];

        //Action
        var encryptedData = _crypto.Encrypt(dataEncrypt, Crypto.Algorithm.Hs512, encryptionKey);
        var decryptedData = _crypto.Decrypt(encryptedData, Crypto.Algorithm.Hs512, encryptionKey);

        CollectionAssert.AreEqual(dataEncrypt, decryptedData, "Decrypted data should match the original data");
    }
}