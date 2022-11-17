using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RegisterAPI.Infrastructure;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

var jwtKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]); 

builder.Services
    .AddAuthentication( options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters{
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddDbContext<DataContext>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention();
});

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "policy",
        policy =>
        {
            policy.WithOrigins("*");
        });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v0",
        new OpenApiInfo
        {
            Title = "Register API",
            Version = "v0",
            Contact = new OpenApiContact { Name = "Igor Couto", Email = "igor.fcouto@gmail.com" }
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

var app = builder.Build();

app.UseCors();

app.MapHealthChecks("/health");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v0/swagger.json", "Register API v0");
    options.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();