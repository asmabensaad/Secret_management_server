using Core.Security;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Services.Kms;

public class Startup
{
    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    readonly ConfigurationOptions _config = new()
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
        // services.AddHttpClient<IKmsVaultClient, KmsVaultClient>();
        services.AddHangfire(configuration => configuration.UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseRedisStorage(ConnectionMultiplexer.Connect(_config)));
        services.AddHangfireServer();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(options => options.AddPolicy(name:MyAllowSpecificOrigins ,
            policy =>
        {
            policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
        }));
    }


    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        
        if (app.Environment.IsDevelopment())
        {
       
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(MyAllowSpecificOrigins);
        app.UseAuthorization();
        app.UseHangfireDashboard("/jobs");
        app.MapHangfireDashboard();

        app.MapControllers();
        app.Run();
        
    }
}