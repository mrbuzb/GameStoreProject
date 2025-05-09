
using GameStore.Bll.Services;
using GameStore.Dal;
using GameStore.Repository.Services;
using GameStore.Server.Configurations;
using GameStore.Server.Middlewares;

namespace GameStore.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.ConfigureDatabase();
            builder.ConfigureDependencies();
            builder.ConfigureLogger();
            builder.Services.AddMemoryCache();
            var app = builder.Build();
            app.UseMiddleware<GlobalExeptionHandling>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseResponseCaching();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
