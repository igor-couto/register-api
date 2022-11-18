using FluentMigrator;

namespace RegisterAPIMigrations;

[Migration(3)]
public class CreateUserTable : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id")
                .AsGuid()
                .NotNullable()
                .PrimaryKey()
            .WithColumn("user_name")
                .AsString(64)
                .NotNullable()
            .WithColumn("first_name")
                .AsString(64)
                .NotNullable()
            .WithColumn("last_name")
                .AsString(64)
                .NotNullable()
            .WithColumn("email")
                .AsString(256)
                .NotNullable()
            .WithColumn("is_email_confirmed")
                .AsBoolean().
                NotNullable()
            .WithColumn("is_locked")
                .AsBoolean()
                .WithDefaultValue(false)
                .NotNullable()
            .WithColumn("phone_number")
                .AsString(16)
                .NotNullable()
            .WithColumn("created_at")
                .AsDate()
                .NotNullable()
            .WithColumn("updated_at")
                .AsDate()
                .Nullable()
            .WithColumn("password_hash")
                .AsString()
                .NotNullable()
            .WithColumn("password_salt")
                .AsString(6)
                .NotNullable()
            .WithColumn("role")
                .AsByte()
                .NotNullable()
                .ForeignKey("role", "id")
            .WithColumn("user_group_id")
                .AsGuid()
                .Nullable()
                .ForeignKey("user_group", "id");
    }

    public override void Down() => Delete.Table("users");
}