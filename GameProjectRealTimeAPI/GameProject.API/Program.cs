using GameProject.Application.Hubs;
using GameProject.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddSingleton<GameSessionService>();
builder.Services.AddSingleton<GameService>();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.UseRouting();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Iniciando requisição: {Method} {Url}", context.Request.Method, context.Request.Path);
        await next.Invoke();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro durante o processamento da requisição: {Method} {Url}", context.Request.Method, context.Request.Path);
        throw;
    }
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<GameHub>("/gameHub"); 
});

app.Run();
