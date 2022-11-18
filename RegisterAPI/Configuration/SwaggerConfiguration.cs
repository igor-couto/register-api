using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace RegisterAPI.Configuration;

public static class SwaggerConfiguration
{
    public static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v0",
                new OpenApiInfo
                {
                    Title = "Register API",
                    Description = "User registration and more.",
                    Version = "v0",
                    Contact = new OpenApiContact { Name = "Igor Couto", Email = "igor.fcouto@gmail.com", Url =new Uri("https://github.com/igor-couto") },
                    License = new OpenApiLicense {Name = "GNU General Public License V3", Url = new Uri("https://github.com/igor-couto/register-api/blob/main/LICENCE")}
                });

            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() 
                { 
                    Name = "Authorization", 
                    Type = SecuritySchemeType.ApiKey, 
                    Scheme = "Bearer", 
                    BearerFormat = "JWT", 
                    In = ParameterLocation.Header, 
                    Description = "JWT Authorization header using the Bearer scheme.\n Enter 'Bearer' [space] and then your token in the text input below.\nExample: \"Bearer 12345abcdef\""
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement 
                { 
                    { 
                            new OpenApiSecurityScheme 
                            { 
                                Reference = new OpenApiReference 
                                { 
                                    Type = ReferenceType.SecurityScheme, 
                                    Id = "Bearer" 
                                } 
                            }, 
                            new string[] {} 
                    } 
                }); 
        });
    }

    public static void UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v0/swagger.json", "Register API v0");
            options.DefaultModelsExpandDepth(-1);
            options.DocExpansion(DocExpansion.None);
            options.DisplayRequestDuration();
        });
    }
}