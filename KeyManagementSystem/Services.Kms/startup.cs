using Core.Security;

namespace Services.Kms;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IKmsVaultClient, KmsVaultClient>();

        services.AddControllers();

    }
}