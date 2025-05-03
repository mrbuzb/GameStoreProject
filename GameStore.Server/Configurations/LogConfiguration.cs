using Serilog;

namespace GameStore.Server.Configurations;

public static class LogConfiguration
{
    public static void ConfigureLogger(this WebApplicationBuilder builer)
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builer.Configuration).CreateLogger();

        builer.Logging.ClearProviders();
        builer.Logging.AddSerilog();
    }
}
