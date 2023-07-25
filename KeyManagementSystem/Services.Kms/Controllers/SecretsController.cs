using Core.Security;
using DataAccess.Models.Kms;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Services.Kms.Controllers;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
[ApiController]
[Route("/api/v1.0/kms/[controller]/[action]")]
public class SecretsController : ControllerBase
{
    private KmsVaultClient _client = new KmsVaultClient();

    /// <summary>
    /// CreateAsyncSecret
    /// </summary>
    /// <param name="secret1"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> CreateAsyncSecret([FromBody] SecretModel secret1, CancellationToken cancellationToken)
    {
        _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200");
        _client.SetUserName(username: "admin");
        _client.SetPassword(password: "admin");

        return await _client.CreatesecretAsync(secret1.Secret, secret1.SecretValue, path: "/kms");
    }

    /// <summary>
    /// GetSecretAsync
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("SecretModel/{secret.key}")]
    public async Task<string> GetSecretAsynch([FromBody] string secret, CancellationToken cancellationToken)
    {
        _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200");
        _client.SetUserName(username: "admin");
        _client.SetPassword(password: "admin");
        return await _client.GetSecretAsync(secret, path: "/kms");
    }

    /// <summary>
    /// DeleteSecretAsync
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("kms/secretModel/{secret}")]
    public async Task<bool> DeleteSecretAsync([FromBody] SecretModel model, CancellationToken cancellationToken)
    {
        _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200");
        _client.SetUserName(username: "admin");
        _client.SetPassword(password: "admin");
        try
        {
            return await _client.DeleteSecretAsync(model.Secret, secretPath: "/kms");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return false;
    }

    /// <summary>
    /// UpdateSecretASync
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> UpdateSecretASync([FromBody] SecretModel model, CancellationToken cancellationToken)
    {
        _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200");
        _client.SetUserName(username: "admin");
        _client.SetPassword(password: "admin");
        var secretValue = new Dictionary<string, object> { { "feel", "good" } };
        if (model.Secret!= null)
        {
            return await _client.UpdateSecretAsync(model.Secret, secretValue, path: "/kms");
        }

        return false;
    }
}