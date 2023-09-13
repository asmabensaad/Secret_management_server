namespace Services.Kms.BL;

/// <summary>
/// Key rotation service
/// </summary>
public interface IKeyRotationService
{
    /// <summary>
    /// Rotate key
    /// </summary>
    /// <param name="keyAlias">Desired key alias</param>
    /// <param name="path">Desired key path</param>
    /// <returns></returns>
    Task RotateAsync(string keyAlias, string path);
}