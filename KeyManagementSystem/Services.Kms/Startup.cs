using Core.Security;
using Hangfire;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;

namespace Services.Kms;

//TODO: Follow naming convention
//TODO: Remove commented code
//TODO: Fix spelling
public class Startup
{
    private const string CorsPolicy = "CORS";

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
        services.AddScoped<IKmsVaultClient, KmsVaultClient>();
        services.AddHangfire(configuration => configuration.UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseRedisStorage(ConnectionMultiplexer.Connect(_config)));
        services.AddHangfireServer();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

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

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(CorsPolicy);
        app.UseAuthorization();
        app.UseHangfireDashboard("/jobs");

        app.UseEndpoints(builder => builder.MapControllers());
    }
}