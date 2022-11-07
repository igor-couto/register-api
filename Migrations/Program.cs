using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var app = Host.CreateDefaultBuilder(args).ConfigureServices( (_, services) => {
    services.AddFluentMigratorCore()
            .ConfigureRunner(
                runnerBuilder =>
                {
                    runnerBuilder
                        .AddPostgres()
                        .WithGlobalConnectionString("Server=localhost;Database=postgres;Port=5432;User Id=admin;Password=admin;")
                        .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()
                        .For.All();
                })
            .AddLogging(lb => lb.AddFluentMigratorConsole());
}).Build();

using var scope = app.Services.CreateScope();
var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();
migrator.MigrateUp();

app.Run();