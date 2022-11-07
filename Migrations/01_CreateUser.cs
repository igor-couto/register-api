using FluentMigrator;

namespace RegisterAPIMigrations;

[Migration(1)]
public class CreateClosedDateTable : Migration
{
    public override void Up()
    {
        Create.Table("user")
            .WithColumn("id").AsDate().NotNullable().PrimaryKey()
            .WithColumn("email").AsDate().NotNullable()
            .WithColumn("phoneNumber").AsString(16).NotNullable()
            .WithColumn("created_at").AsDate().NotNullable()
            .WithColumn("updated_at").AsDate().NotNullable();
    }

    public override void Down() => Delete.Table("user");
}