using Microsoft.EntityFrameworkCore;
using RegisterAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionStrings"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();