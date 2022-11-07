using Microsoft.EntityFrameworkCore;
using RegisterAPI.Domain;

namespace RegisterAPI.Infrastructure;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) {}

    public DbSet<User> Users {get; set;}
}