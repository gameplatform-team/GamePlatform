using GamePlatform.Api.Configuration;
using GamePlatform.Api.Middlewares;
using GamePlatform.Application.Configuration;
using GamePlatform.Infrastructure.Contexts;
using GamePlatform.Infrastructure.Seed;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCustomHttpLogging();

// JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Adicionar configuracoes do banco de dados e servicos da infraestrutura
builder.Services.AddInfrastructureServices(builder.Configuration);

// Adicionar servicos da camada de aplica��o
builder.Services.AddApplicationServices();

// Controllers e Swagger
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithOptions();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Middleware de log
app.UseHttpLogging();

// Chamar o Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();

    await DbInitializer.SeedAsync(context);
}

// Middleware do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares HTTP
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
