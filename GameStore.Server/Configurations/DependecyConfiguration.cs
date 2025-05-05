using GameStore.Bll.Mappers;
using GameStore.Bll.Services;
using GameStore.Dal;
using GameStore.Repository.Services;

namespace GameStore.Server.Configurations;

public static class DependecyConfiguration
{
    public static void ConfigureDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IGenreRepository, GenreRepository>();
        builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
        builder.Services.AddScoped<IGameService, GameService>();
        builder.Services.AddScoped<IGenreService, GenreService>();
        builder.Services.AddScoped<IPlatformService, PlatformService>();
        builder.Services.AddScoped<MainContext>();
        builder.Services.AddSingleton(new TelegramLogger("8058296814:AAGmLEzCcbukiRiqqqh7IW6Oh4YfXQ6YvkM", "-1002579719825"));
        builder.Services.AddAutoMapper(typeof(GameProfile));
        builder.Services.AddAutoMapper(typeof(GenreProfiles));
        builder.Services.AddAutoMapper(typeof(PlatformProfile));
    }
}
