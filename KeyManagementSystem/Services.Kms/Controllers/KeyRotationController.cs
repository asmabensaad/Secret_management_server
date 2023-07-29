using Core.Security;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Services.Kms.Controllers;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
[ApiController]
[Route("/api/v1.0/kms/[controller]/[action]")]
public class KeyRotationController : ControllerBase
{
    private readonly KmsVaultClient _client = new KmsVaultClient();


    /// <summary>
    /// KeyRotateRecuringJob
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="secretValue"></param>
    /// <returns></returns>
    [HttpPut]
    public IActionResult KeyRotateRecuringJob([FromBody] string secret,Dictionary<string, object> secretValue)
    {
        var jobId = BackgroundJob.Enqueue(() => ProcessKeyAsync(secret,secretValue));

        return Accepted(jobId);
    }

    /// <summary>
    /// ProcessKeyAsync
    /// </summary>
    /// <param name="secret"></param>
    /// <param name="secretValue"></param>
    /// <returns></returns>
    [HttpPut]
    public Task<bool> ProcessKeyAsync(string secret,Dictionary<string, object> secretValue)
    {
        return _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200")
            .SetUserName(username: "admin")
            .SetPassword(password: "admin")
            .RecurringJobsRotateKeyAsync(secret, "/kms",secretValue);
    }
}