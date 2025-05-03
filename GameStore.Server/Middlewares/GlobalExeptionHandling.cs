using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using GameStore.Bll.Services;
using System.Net;
using System.Text.Json;

namespace GameStore.Server.Middlewares;

public class GlobalExeptionHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExeptionHandling> _logger;
    private readonly TelegramLogger telegramLogger;

    public GlobalExeptionHandling(RequestDelegate next, TelegramLogger telegramLogger, ILogger<GlobalExeptionHandling> logger)
    {
        _next = next;
        this.telegramLogger = telegramLogger;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
                catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await telegramLogger.LogAsync(ex.ToString());

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = context.Response.StatusCode,
                message = "Internal Server Error",
                detail = ex.Message
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
