using System.Security.Cryptography;
using Core.Security.Vault;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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

    //  public SecretModel model;

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
        var vaultClientSettings = new VaultClientSettings($"{VaultAddress}:{Port}", userpass);
        IVaultClient vaultClient = new VaultClient(vaultClientSettings);
        return vaultClient;
    }

    /// <inheritdoc cref="IKmsVaultClient.RotateAsync"/>
    public async Task RotateAsync(string alias, string path)
    {
        var newKey = Convert.ToBase64String(GetRandomBytes(32));

        await GetClient().V1.Secrets.KeyValue.V2.WriteSecretAsync(alias,
            new Dictionary<string, object> {{"secretValue", newKey}}, mountPoint: path);
    }


    /// <inheritdoc cref="IKmsVaultClient.GetSecretAsync"/>
    public async Task<Secret<SecretData>> GetSecretAsync(string key, string path)
    {
        var client = GetClient();

        try
        {
            return await client.V1.Secrets.KeyValue.V2.ReadSecretAsync(key, mountPoint: path);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <inheritdoc cref="IKmsVaultClient.CreatesecretAsync"/>
    public async Task<Secret<SecretData>> CreatesecretAsync(string key, Dictionary<string, object> value, string path)
    {
        try
        {
            var client = GetClient();

            await client.V1.Secrets.KeyValue.V2.WriteSecretAsync(key, value, null, path);
            return await GetSecretAsync(key, path);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <inheritdoc cref="IKmsVaultClient.UpdateSecretAsync"/>
    public async Task<Secret<SecretData>> UpdateSecretAsync(string key, IDictionary<string, object> secretValue,
        string path)
    {
        var client = GetClient();
        try
        {
            await client.V1.Secrets.KeyValue.V2.DeleteSecretAsync(key, path);
            await client.V1.Secrets.KeyValue.V2.WriteSecretAsync(key, secretValue, null, path);
            return await GetSecretAsync(key, path);
        }
        catch (Exception)

        {
            return null;
        }
    }

    /// <inheritdoc cref="IKmsVaultClient.DeleteSecretAsync"/>
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

    /// <inheritdoc cref="IKmsVaultClient.DestroySecretAsync"/>
    public async Task<bool> DestroySecretAsync(string path, IList<int> versionToDelete, string key)
    {
        var client = GetClient();
        // IList<int> version = new List<int> {1};
        try
        {
            var destroyedVersions = new List<int>();
            foreach (var version in versionToDelete)
            {
                await client.V1.Secrets.KeyValue.V2.DestroySecretVersionsAsync(path, versionToDelete, key);
                destroyedVersions.Add(version);
            }

            return destroyedVersions.Count > 0;
        }
        catch (Exception)
        {
            throw new CoreSecurityException("secret not found in vault");
        }
    }

    /// <inheritdoc cref="IKmsVaultClient.GetAllAsync"/>
    public async Task<VaultDataModel> GetAllAsync(string mountPoint)
    {
        try
        {
            if (await GetTokenAsync() is var token && string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            using var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Vault-Token", token);
            var result = await client.GetAsync($"{VaultAddress}:{Port}/v1/{mountPoint}/metadata/?list=true");
            Console.WriteLine(result.StatusCode);

            var responseBody = await result.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<VaultListSecretModel>(responseBody);

            return response.VaultData;
        }
        catch (HttpRequestException)
        {
            throw new BadHttpRequestException("bad request");
        }
    }

    /// <summary>
    /// GetTokenAsync 
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetTokenAsync()
    {
        try
        {
            using var client = new HttpClient();

            client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json; charset=utf-8");

            var result = await client.PostAsync($"{VaultAddress}:{Port}/v1/auth/userpass/login/{Username}",
                new StringContent(JsonConvert.SerializeObject(new
                {
                    password = Password
                })));

            Console.WriteLine(result.StatusCode);

            var responseBody = await result.Content.ReadAsStringAsync();

            var token = JsonConvert.DeserializeObject<VaultTokenResponse>(responseBody);
            return token?.Auth?.ClientToken;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Generate PRN bytes
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static byte[] GetRandomBytes(int length)
    {
        var secret = new byte[length];
        var random = RandomNumberGenerator.Create();
        random.GetBytes(secret);
        return secret;
    }
}