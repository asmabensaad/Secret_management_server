using Core.Security.Vault;
using VaultSharp;
using VaultSharp.V1.Commons;

namespace Core.Security;

/// <summary>
/// Vault client
/// </summary>
public interface IKmsVaultClient
{
    /// <summary>
    /// username
    /// </summary>
    public string Username { get; internal set; }

    /// <summary>
    /// password
    /// </summary>
    public string Password { get; internal set; }

    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; internal set; }

    /// <summary>
    /// VaultAddress
    /// </summary>
    public string VaultAddress { get; internal set; }

    /// <summary>
    /// Set user name
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns></returns>
    public IKmsVaultClient SetUserName(string username);

    /// <summary>
    /// Set password
    /// </summary>
    /// <param name="password">Password</param>
    /// <returns></returns>
    public IKmsVaultClient SetPassword(string password);

    /// <summary>
    /// set Port
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public IKmsVaultClient SetPort(int port);

    /// <summary>
    /// set address
    /// </summary>
    /// <param name="vaultAddress"></param>
    /// <returns></returns>
    public IKmsVaultClient SetVaultAddress(string vaultAddress);

    /// <summary>
    /// GetSecret
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<Secret<SecretData>> GetSecretAsync(string key, string path);


    /// <summary>
    /// Createsecret
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="path"></param>
    public Task<Secret<SecretData>> CreatesecretAsync(string key, Dictionary<string, object> value, string path);

    /// <summary>
    /// UpdateSecret
    /// </summary>
    /// <param name="key"></param>
    /// <param name="secretValue"></param>
    /// <param name="path"></param>
    public Task<Secret<SecretData>> UpdateSecretAsync(string key, IDictionary<string, object> secretValue, string path);

    /// <summary>
    /// DeleteSecret
    /// </summary>
    /// <param name="key"></param>
    /// <param name="secretPath"></param>
    public Task<bool> DeleteSecretAsync(string key, string secretPath);

    /// <summary>
    /// GetClient
    /// </summary>
    /// <returns></returns>
    public IVaultClient GetClient();

    /// <summary>
    /// Rotate key
    /// </summary>
    /// <param name="alias">Key alias name</param>
    /// <param name="path">Key path</param>
    /// <returns></returns>
    public Task RotateAsync(string alias, string path);

    //TODO: Document the code.
    public Task<bool> DestroySecretAsync(string path, IList<int> versionToDelete, string key);

    /// <summary>
    /// Get All Secret From Vault 
    /// </summary>
    /// <param name="mountPoint"></param>
    /// <returns></returns>
    public Task<VaultDataModel> GetAllAsync(string mountPoint);
}