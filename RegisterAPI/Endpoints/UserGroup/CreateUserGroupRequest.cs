namespace RegisterAPI.Domain.Requests;

public class CreateUserGroupRequest
{
    public string Name { get; init; }

    public UserGroup Create()
    {
        return new UserGroup()
        {
            Id = Guid.NewGuid(),
            Name = Name,
            CreatedAt = DateTime.Now
        };
    }
}