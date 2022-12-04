using Microsoft.EntityFrameworkCore;

namespace RegisterAPI.Infrastructure;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) {}

    public DbSet<User> Users {get; set;}
    public DbSet<UserGroup> UserGroups {get; set;}
}