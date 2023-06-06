using Core.Security.Cryptographie;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Security.Tests;

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
        Assert.IsNotNull(Crypto.EncryptData(Crypto.algorithm.ECDH));

    }


[TestMethod]
    public void B_DecryptionTest()
    {
        Assert.IsNotNull(Crypto.Decryptdata(Crypto.algorithm.ECDH));
        
    }
}
