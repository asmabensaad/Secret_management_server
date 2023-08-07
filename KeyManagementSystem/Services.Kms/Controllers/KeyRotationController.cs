using Core.Security;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Services.Kms.Controllers;

[ApiController]
[Route("/api/v1.0/kms/[controller]/[action]")]
public class KeyRotationController : ControllerBase
{
    private readonly KmsVaultClient _client;

    public KeyRotationController(KmsVaultClient client)
    {
        _client = client;
    }


    /// <summary>
    /// KeyRotateRecuringJob
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="secretValue"></param>
    /// <returns></returns>
    [HttpPut]
    public IActionResult KeyRotateRecuringJob([FromQuery] string secret,
        [FromBody] Dictionary<string, object> secretValue)
    {
        var jobId = BackgroundJob.Enqueue(() => ProcessKeyAsync(secret, secretValue));

        return Accepted(jobId);
    }

    /// <summary>
    /// ProcessKeyAsync
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="secretValue"></param>
    /// <returns></returns>
    [HttpPut]
    public Task<bool> ProcessKeyAsync(string secret, Dictionary<string, object> secretValue)
    {
        return _client.RecurringJobsRotateKeyAsync(secret, "/kms", secretValue);
    }
}