using Localidades.Application.Configurations;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Localidades.Application.Services;

public static class AzureKeyVaultExtensions
{
    public static SecretClient AzureSecretClient(this WebApplicationBuilder builder)
    {
        SecretClientOptions options = new SecretClientOptions()
        {
            Retry =
            {
                Delay= TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };

        var keyVaultUrl = builder.Configuration["KeyVaultUrl"];
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
        return new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential(), options);
    }

    public static void SetAppSecrets(this SecretClient client)
    {
        Settings.SetSecret(GetSecretByName(client, "secret").Value);
        Settings.SetConnectionString(GetSecretByName(client, "connectionString").Value);
    }

    public static KeyVaultSecret GetSecretByName(this SecretClient client, string secretName)
    {
        return client.GetSecret(secretName);
    }
}

