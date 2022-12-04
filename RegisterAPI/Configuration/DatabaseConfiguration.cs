using Microsoft.EntityFrameworkCore;

namespace RegisterAPI.Configuration;

public static class DatabaseConfiguration
{
    public static void RegisterDatabase(this IServiceCollection services, string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddDbContext<DataContext>(options =>
        {
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention();
        });
    }
}