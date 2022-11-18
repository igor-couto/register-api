using System.Runtime.Serialization;

namespace RegisterAPI.Domain;

public enum Role
{
    [EnumMember(Value = "admin")] Admin,
    [EnumMember(Value = "user")] User
}