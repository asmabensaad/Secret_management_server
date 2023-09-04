using Core.Security;
using Core.Security.Cryptographie;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StackExchange.Redis;

namespace Services.Kms;

public class Startup
{
    private const string CorsPolicy = "CORS";

    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private readonly ConfigurationOptions _config = new()
    {
        KeepAlive = 0,
        AllowAdmin = true,
        EndPoints = { { "127.0.0.1", 6379 } },
        ConnectTimeout = 5000,
        ConnectRetry = 3,
        SyncTimeout = 5000,
        AbortOnConnectFail = false,
        Password = "eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81"
    };

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        var vaultConfig = _configuration.GetSection("VaultConfig").Get<KmsVaultClient>();

        services.AddScoped<IKmsVaultClient, KmsVaultClient>(
            _ => new KmsVaultClient
            {
                VaultAddress = vaultConfig.VaultAddress,
                Username = vaultConfig.Username,
                Password = vaultConfig.Password,
                Port = vaultConfig.Port
            });

        services.AddHangfire(configuration => configuration.UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseRedisStorage(ConnectionMultiplexer.Connect(_config)));
        services.AddHangfireServer();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMvcCore()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            });
        services.AddScoped<ICrypto, Crypto>();
       


        services.AddCors(options => options.AddPolicy(name: CorsPolicy,
            policy => { policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin(); }));
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors(CorsPolicy);

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(builder => builder.MapControllers());

        app.UseHangfireDashboard("/jobs");
    }
}