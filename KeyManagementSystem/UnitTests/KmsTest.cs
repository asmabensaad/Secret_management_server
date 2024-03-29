using Core.Security;
using VaultSharp;

namespace UnitTests;

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
            var client = _kmsVaultClient.SetPassword(password: "admin").SetUserName("admin").GetClient();
            //Assert
            Assert.IsInstanceOfType(client, typeof(IVaultClient));
        }
        // ReSharper disable once EmptyGeneralCatchClause
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }

    /// <summary>
    /// TestCreatesecret
    /// </summary>
    [TestMethod]
    public async Task B_TestCreatesecretAsync()
    {
        //Arrange
        const string path = "kms/";
        var secretValue = new Dictionary<string, object>
        {
            {"username", "testuser"},
            {"password", "testpassword"}
        };
        //Action
        try
        {
            _kmsVaultClient.SetPassword(password: "admin")
                .SetUserName("admin")
                .SetVaultAddress("http://127.0.0.1:8200");

            //Assert 
            Assert.IsNotNull(await _kmsVaultClient.CreatesecretAsync("first",
                secretValue, path));
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

            Assert.IsNotNull(await _kmsVaultClient.GetSecretAsync(key, path));
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
        const string key = "first";
        const string path = "/kms";
        var secretvalue = new Dictionary<string, object> {{"feel", "good"}};

        try
        {
            _kmsVaultClient.SetUserName("admin").SetPassword("admin").SetVaultAddress("http://127.0.0.1:8200");

            var result = await _kmsVaultClient.UpdateSecretAsync(key, secretvalue
                , path);
            Assert.IsNotNull(result);
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
        const string secretpath = "/kms";
        const string key = "aa";
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

    /// <summary>
    /// F_RecuringJobTest
    /// </summary>
    [TestMethod]
    public async Task F_RecuringJobTest()
    {
        const string path = "/kms";
        var secretValue = new Dictionary<string, object> {{"new-secret", "new-value"}};
        _kmsVaultClient.SetUserName("admin").SetPassword("admin").SetVaultAddress("http://127.0.0.1:8200");
        Assert.IsTrue(await _kmsVaultClient.RecurringJobsRotateKeyAsync(key: "first", path, secretValue));
    }
}