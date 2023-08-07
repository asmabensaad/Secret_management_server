using Core.Security;
using DataAccess.Models.Api;
using DataAccess.Models.Kms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VaultSharp.V1.Commons;

namespace Services.Kms.Controllers;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
[ApiController]
[Route("/api/v1.0/kms/[controller]/[action]")]
public class SecretsController : ControllerBase
{
    private readonly IKmsVaultClient _client;

    public SecretsController(IKmsVaultClient client)
    {
        _client = client;
    }


    /// <summary>
    /// CreateAsyncSecret
    /// </summary>
    /// <param name="secret1"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsyncSecret([FromBody] SecretModel secret1,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Error = ApiError.InvalidData
            });
        }

        return Created(string.Empty,
            new ApiResponse<Secret<SecretData>>
            {
                Data = await _client.CreatesecretAsync(secret1.Secret, secret1.SecretValue, path: "/kms")
            });
    }

    /// <summary>
    /// GetSecretAsync
    /// </summary>
    /// <param name="alias">Key alias</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetSecretAsync([FromQuery(Name = "alias"), BindRequired] string alias,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<object>
            {
                Error = ApiError.InvalidData
            });
        }

        if (await _client.GetSecretAsync(alias, path: "/kms") is var secret && secret == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Error = ApiError.SecretNotFound
            });
        }

        return Ok(secret);
    }

    /// <summary>
    /// DeleteSecretAsync
    /// </summary>
    /// <param name="alias">Key alias</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteSecretAsync([FromQuery(Name = "alias"), BindRequired] string alias,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }

        await _client.DeleteSecretAsync(alias, secretPath: "/kms");

        return NoContent();
    }

    /// <summary>
    /// UpdateSecretASync
    /// </summary>
    /// <param name="secretValue"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="keyalias"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<IActionResult> UpdateSecretASync([FromQuery(Name = "keyAlias")] string keyalias,
        [FromBody, BindRequired] Dictionary<string, object> secretValue,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data");
        }


        return Ok(await _client.UpdateSecretAsync(keyalias, secretValue, path: "/kms"));
    }

    /// <summary>
    /// DestroySecretAsync
    /// </summary>
    /// <param name="versionToDestroy"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<IActionResult> DestroySecretAsync(
        [FromQuery] IList<int> versionToDestroy, [FromQuery] string key,
        CancellationToken cancellationToken)
    {
        return Ok(await _client.DestroySecretAsync(path: "/kms", versionToDestroy, key));
    }


    [HttpGet]
    public async Task<IActionResult> GetAllSecretAsync([FromQuery(Name = "mountPoint"), BindRequired] string mountPoint)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        return Ok(await _client.GetAllAsync(mountPoint));
    }
}