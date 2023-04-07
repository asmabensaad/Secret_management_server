using Hangfire;
using Hangfireapp.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IserviceManagement, ServiceManagement>();
var config = new ConfigurationOptions()
{
    KeepAlive = 0,
    AllowAdmin = true,
    EndPoints = { { "127.0.0.1", 6379 },{ "127.0.0.2", 6379 } },
    ConnectTimeout = 5000,
    ConnectRetry = 3,
    SyncTimeout = 5000,
    AbortOnConnectFail = false,
    Password = "eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81"
};
IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);

builder.Services.AddScoped(s => redis.GetDatabase());
builder.Services.AddMemoryCache();

builder.Services.AddHangfire(configuration => configuration.UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().
    UseRedisStorage("localhost:6379 , Password=eYVX7EwVmmxKPCDmwMtyKVge8oLd2t81"));
builder.Services.AddHangfireServer();


var app = builder.Build();
//await app.WaitForShutdownAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/dashborad");

app.MapHangfireDashboard();
app.Run();