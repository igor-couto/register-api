using RegisterAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder);

await using var app = builder.Build();

ConfigureApplication(app);

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.RegisterFluentMigrator(connectionString);

    builder.Services.RegisterDatabase(connectionString);

    builder.Services.RegisterAuth(builder.Configuration);

    builder.Services.RegisterHealthCheck();

    builder.Services.RegisterCors();

    builder.Services.RegisterSwagger();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddControllers();
}

static void ConfigureApplication(WebApplication app)
{
    app.UseCors();

    app.UseHealthCheckConfiguration();

    app.UseDeveloperExceptionPage();

    app.UseHsts();

    app.UseSwaggerConfiguration();

    app.UseHttpsRedirection();

    app.UseAuthConfiguration();

    app.MapControllers();

    app.UseFluentMigratorConfiguration();
}

public partial class Program { }