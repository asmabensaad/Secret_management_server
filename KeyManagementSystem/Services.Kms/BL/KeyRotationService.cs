namespace Services.Kms.BL;

/// <inheritdoc cref="IKeyRotationService"/>
public class KeyRotationService: IKeyRotationService
{
    /// <inheritdoc cref="IKeyRotationService.RotateAsync"/>
    public Task RotateAsync(string keyAlias)
    {
        throw new NotImplementedException();
    }
}