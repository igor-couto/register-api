namespace RegisterAPI.Endpoints.Authorization;

public class LoginResult
{
    public string AccessToken { get; init; }
    public string TokenType { get; init; } =  "Bearer";
    public string ExpiresAfter { get; init; }
}