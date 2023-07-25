using Core.Security;
using Core.Security.Cryptographie;

namespace UnitTests;

[TestClass]
public class CryptoTest
{
    readonly KmsVaultClient _kmsVaultClient = new();

    public enum algorithm
    {
        ECDH,
        HS256,
        AES
    }

    [TestMethod]
    public void A_EncryptionTest()
    {

        _kmsVaultClient.SetUserName("admin").SetPassword("admin").SetVaultAddress("http://127.0.0.1:8200");
        Assert.IsNotNull(Crypto.EncryptData(Crypto.Algorithm.Ecdh));

    }


[TestMethod]
    public void B_DecryptionTest()
    {
        Assert.IsNotNull(Crypto.Decryptdata(Crypto.Algorithm.Ecdh));
        
    }
}
