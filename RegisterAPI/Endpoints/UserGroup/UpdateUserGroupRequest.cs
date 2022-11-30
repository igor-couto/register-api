namespace RegisterAPI.Domain.Requests;

public class UpdateUserGroupRequest
{
    public string Name { get; set; }
    public int NumberOfMembers { get; set; }

    public UserGroup Update(UserGroup userGroup)
    {
        userGroup.Name = Name;
        userGroup.NumberOfMembers = NumberOfMembers;
        userGroup.UpdatedAt = DateTime.Now;

        return userGroup;
    }
}