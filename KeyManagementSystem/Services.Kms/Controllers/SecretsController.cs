using Core.Security;
using DataAccess.Models.Kms;
using Microsoft.AspNetCore.Mvc;

namespace Services.Kms.Controllers;

[ApiController]
[Route("/api/v1.0/kms/[controller]/[action]")]
public class SecretsController : ControllerBase
{
    private KmsVaultClient _client = new KmsVaultClient();

    /// <summary>
    /// CreateAsyncSecret
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> CreateAsyncSecret([FromBody] SecretModel secret, CancellationToken cancellationToken)
    {
        _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200");
        _client.SetUserName(username: "admin");
        _client.SetPassword(password: "admin");

        return await _client.CreatesecretAsync(secret.Key, secret.Secretvalue, path: "/kms");
    }

    /// <summary>
    /// GetSecretAsync
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("SecretModel{secret.key}")]
    public async Task<string> GetSecretAsync([FromBody] string key, CancellationToken cancellationToken)
    {
        _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200");
        _client.SetUserName(username: "admin");
        _client.SetPassword(password: "admin");
        return await _client.GetSecretAsyn(key, path: "/kms");
    }

    /// <summary>
    /// DeleteSecretAsync
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("kms/secretModel/{secret}")]
    public async Task<bool> DeleteSecretAsync([FromBody] SecretModel secret, CancellationToken cancellationToken)
    {
        _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200");
        _client.SetUserName(username: "admin");
        _client.SetPassword(password: "admin");
        try
        {
            return await _client.DeleteSecretAsync(secret.Key, secretPath: "/kms");
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
    /// <param name="secret"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> UpdateSecretASync([FromBody] SecretModel secret, CancellationToken cancellationToken)
    {
        _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200");
        _client.SetUserName(username: "admin");
        _client.SetPassword(password: "admin");
        var secretValue = new Dictionary<string, object> { { "feel", "good" } };
        if (secret.Key != null)
        {
            return await _client.UpdateSecretAsync(secret.Key, secretValue, path: "/kms");
        }

        return false;
    }
}