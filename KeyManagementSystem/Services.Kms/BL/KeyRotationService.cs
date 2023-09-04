using Core.Security;
using Hangfire;

namespace Services.Kms.BL;

/// <inheritdoc cref="IKeyRotationService"/>
public class KeyRotationService : IKeyRotationService
{
    private readonly IKmsVaultClient _client;

    public KeyRotationService(IKmsVaultClient client)
    {
        _client = client;
    }

    /// <inheritdoc cref="IKeyRotationService.RotateAsync"/>
    public Task RotateAsync(string keyAlias, string path)
    {
        BackgroundJob.Enqueue(() => _client.RotateAsync(keyAlias, path));
        return Task.CompletedTask;
    }
}