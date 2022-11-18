namespace RegisterAPI.Domain.Requests;

public class UpdateUserRequest
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsLocked { get; set; }

    public User Update(User user)
    {
        user.UserName = UserName;
        user.FirstName = FirstName;
        user.LastName = LastName;
        user.Email = Email;
        user.IsEmailConfirmed = IsEmailConfirmed;
        user.IsLocked = IsLocked;
        user.UpdatedAt = DateTime.Now;

        return user;
    }
}