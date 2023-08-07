namespace Services.Kms;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = CreateHostBuilder(args);
        var app = builder.Build();
        app.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls($"http://0.0.0.0:2000");
            });
}