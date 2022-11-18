namespace RegisterAPI.Configuration;

public static class CorsConfiguration
{
    public static void RegisterCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                name: "policy",
                policy =>
                {
                    policy.WithOrigins("*");
                });
        });
    }
}
