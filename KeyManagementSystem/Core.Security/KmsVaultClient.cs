using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.UserPass;
using VaultSharp.V1.Commons;

namespace Core.Security;

/// <inheritdoc cref="IKmsVaultClient"/>
public class KmsVaultClient : IKmsVaultClient
{
    /// <inheritdoc cref="IKmsVaultClient.Username"/>
    public string Username { get; set; }

    /// <inheritdoc cref="IKmsVaultClient.Password"/>
    public string Password { get; set; }

    /// <inheritdoc cref="IKmsVaultClient.Port"/>
    public int Port { get; set; }

    /// <inheritdoc cref="IKmsVaultClient.VaultAddress"/>
    public string VaultAddress { get; set; }

    /// <inheritdoc cref="IKmsVaultClient.SetUserName"/>
    public IKmsVaultClient SetUserName(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new CoreSecurityException($"{nameof(username)} is required");
        }

        Username = username;
        return this;
    }

    /// <inheritdoc cref="IKmsVaultClient.SetPassword"/>
    public IKmsVaultClient SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new CoreSecurityException($"{nameof(password)} is required");
        }

        Password = password;
        return this;
    }

    /// <inheritdoc cref="IKmsVaultClient.SetPort"/>
    public IKmsVaultClient SetPort(int port)
    {
        if (!port.Equals(0))
        {
            throw new CoreSecurityException($"{nameof(port)} is required");
        }

        Port = port;
        return this;
    }

    /// <inheritdoc cref="IKmsVaultClient.SetVaultAddress"/>
    public IKmsVaultClient SetVaultAddress(string vaultAddress)
    {
        if (string.IsNullOrWhiteSpace(vaultAddress))
        {
            throw new CoreSecurityException($"{nameof(vaultAddress)} is required");
        }

        VaultAddress = vaultAddress;
        return this;
    }


    /// <summary>
    /// GetClient
    /// </summary>
    /// <returns></returns>
    public IVaultClient GetClient()
    {
        if (string.IsNullOrWhiteSpace(VaultAddress))
        {
            throw new CoreSecurityException($"{nameof(VaultAddress)} is required");
        }

        if (string.IsNullOrWhiteSpace(Username))
        {
            throw new CoreSecurityException($"{nameof(Username)} is required");
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            throw new CoreSecurityException($"{nameof(Password)} is required");
        }

        IAuthMethodInfo userpass = new UserPassAuthMethodInfo(username: Username, password: Password);
        var vaultClientSettings = new VaultClientSettings(VaultAddress, userpass);
        IVaultClient vaultClient = new VaultClient(vaultClientSettings);

        return vaultClient;
    }

    /// <summary>
    /// GetSecret
    /// </summary>
    /// <returns></returns>
    /// <exception cref="CoreSecurityException"></exception>
    public async Task<string> GetSecretAsyn(string key, string path)
    {
        var client = GetClient();
        try
        {
            Secret<SecretData> kv2Secret = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync(key, mountPoint: path);
            string s = kv2Secret.Data.ToString();
            return s;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return e.Message;
        }
    }

    /// <summary>
    /// Createsecret
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="path"></param>
    /// <exception cref="CoreSecurityException"></exception>
    public async Task<bool> CreatesecretAsync(string key, Dictionary<string, object> value, string path)
    {
        try
        {
            var client = GetClient();
            await client.V1.Secrets.KeyValue.V2.WriteSecretAsync(key, value, null, path);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    /// <summary>
    /// UpdateSecret
    /// </summary>
    /// <param name="key"></param>
    /// <param name="secretValue"></param>
    /// <param name="path"></param>
    public async Task<bool> UpdateSecretAsync(string key, IDictionary<string, object> secretValue, string path)
    {
        var client = GetClient();
        try
        {
            await client.V1.Secrets.KeyValue.V2.DeleteSecretAsync(key, path);
            await client.V1.Secrets.KeyValue.V2.WriteSecretAsync(key, secretValue, null, path);
            return true;
        }
        catch (Exception e)

        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    /// <summary>
    /// DeleteSecret
    /// </summary>
    /// <param name="key"></param>
    /// <param name="secretPath"></param>
    public async Task<bool> DeleteSecretAsync(string key, string secretPath)
    {
        var client = GetClient();

        try
        {
            await client.V1.Secrets.KeyValue.V2.DeleteSecretAsync(key, secretPath);
            return true;
        }
        catch (Exception)
        {
            throw new CoreSecurityException("path  kms/ not found in vault");
        }
    }
}