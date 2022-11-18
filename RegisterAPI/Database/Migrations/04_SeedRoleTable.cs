using FluentMigrator;
using RegisterAPI.Domain;

namespace RegisterAPIMigrations;

[Migration(4)]
public class SeedRoleTable : Migration
{
    public override void Up()
    {
        Insert.IntoTable("role")
        .Row(new {
            id = (short) Role.Admin,
            role = Role.Admin.ToString()
        })
        .Row(new {
            id = (short) Role.User,
            role = Role.User.ToString()
        });
    }

    public override void Down() 
        => Delete.FromTable("role")
            .Row(new { id = Role.Admin })
            .Row(new { id = Role.User });
}