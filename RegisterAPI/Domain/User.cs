namespace RegisterAPI.Domain;

public class User
{
    public Guid Id;
    public string UserName;
    public string FirstName;
    public string LastName;
    public string Email;
    public bool IsEmailConfirmed;
    public bool IsLocked;
    public string PhoneNumber;
    public DateTime CreatedAt;
    public DateTime  UpdatedAt;
    public string PasswordHash;
}