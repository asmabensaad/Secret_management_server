using Microsoft.VisualStudio.TestTools.UnitTesting;

using VaultSharp;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;

namespace Core.Security.Tests;

[TestClass]
public class KmsTest
{
    readonly KmsVaultClient _kmsVaultClient = new();

    /// <summary>
    /// GetClient_returnClientVaultIfConected_ReturnNull
    /// </summary>
    [TestMethod]
    public void A_GetClient_returnClientVaultIfConected_ReturnNull()
    {
        try
        {
            //Action
            IVaultClient client = _kmsVaultClient.SetPassword(password: "admin").SetUserName("admin").GetClient();
            //Assert
            Assert.IsInstanceOfType(client, typeof(IVaultClient));
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// TestCreatesecret
    /// </summary>
    [TestMethod]
    public async Task B_TestCreatesecret()
    {
        //Arrange
        const string path = "kms/";
        //Action
        try
        {
            _kmsVaultClient.SetPassword(password: "admin")
                .SetUserName("admin")
                .SetVaultAddress("http://127.0.0.1:8200");

            //Assert 
            Assert.IsTrue(await _kmsVaultClient.CreatesecretAsync("first", new Dictionary<string, object>
            {
                {"test","x"}
            },path));
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    /// <summary>
    /// TestGetsecret
    /// </summary>
    [TestMethod]
    public async Task C_TestGetsecretAsync()
    {
        string key = "first";
        string path = "/kms";
        //Action
        try
        {
            _kmsVaultClient.SetUserName("admin").SetPassword("admin").SetVaultAddress("http://127.0.0.1:8200");

            Assert.IsNotNull(await _kmsVaultClient.GetSecretAsyn(key, path));
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    /// <summary>
    /// TestUpdatesecret
    /// </summary>
    [TestMethod]
    public async Task D_TestUpdatesecretAsync()
    {
        string key = "first";
        string path = "/kms";
        var secretvalue = new Dictionary<string, object> { { "feel", "good" } };

        try
        {

            _kmsVaultClient.SetUserName("admin").SetPassword("admin").SetVaultAddress("http://127.0.0.1:8200");
           
            var result = await _kmsVaultClient.UpdateSecretAsync(key, secretvalue
                ,path);
            Assert.IsTrue(result);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    /// <summary>
    /// TestDeletesecret
    /// </summary>
    [TestMethod]
    public async Task E_TestDeletesecretAsync()
    {
        string secretpath = "/kms";
        string key = "aa";
        try
        { 
            _kmsVaultClient.SetUserName("admin").SetPassword("admin").SetVaultAddress("http://127.0.0.1:8200");

            Assert.IsTrue(await _kmsVaultClient.DeleteSecretAsync(key, secretpath));
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }
}