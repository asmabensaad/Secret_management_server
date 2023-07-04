using Core.Security;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Services.Kms.Controllers;

[ApiController]
[Route("/api/v1.0/kms/[controller]/[action]")]
public class KeyRotationController : ControllerBase
{
    private KmsVaultClient _client = new KmsVaultClient();


    /// <summary>
    /// KeyRotateRecuringJob
    /// </summary>
    /// <param name="secret"></param>
    /// <returns></returns>
    [HttpPut]
    public IActionResult KeyRotateRecuringJob([FromBody] string secret)
    {
        var jobId = BackgroundJob.Enqueue(() => ProcessKeyAsync(secret));

        return Accepted(jobId);
    }

    /// <summary>
    /// ProcessKeyAsync
    /// </summary>
    /// <param name="secret"></param>
    /// <returns></returns>
    [HttpPut]
    public Task<bool> ProcessKeyAsync(string secret)
    {
        return _client.SetVaultAddress(vaultAddress: "http://127.0.0.1:8200")
            .SetUserName(username: "admin")
            .SetPassword(password: "admin")
            .RecurringJobsRotateKey(secret, "/kms");
    }
}