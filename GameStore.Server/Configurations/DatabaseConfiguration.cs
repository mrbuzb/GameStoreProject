using GameStore.Dal;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Server.Configurations;

public static class DatabaseConfiguration
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DataBaseConnection");
        builder.Services.AddDbContext<MainContext>(options =>
                 options.UseSqlServer(connectionString));
    }
}
