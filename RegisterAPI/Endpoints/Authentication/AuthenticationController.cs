using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scrypt;
using RegisterAPI.Endpoints.Authorization;

namespace user_api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly DataContext _dataContext;
    private SymmetricSecurityKey _secretKey;
    private string _issuer;
    private string _audience;

    public AuthenticationController(IConfiguration configuration, ILogger<UsersController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;

        _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var user = await FindUserFromRequest(loginRequest, cancellationToken);

        if(user is null)
            return NotFound("User not found");

        var isPasswordValid = new ScryptEncoder().Compare(user.PasswordSalt + loginRequest.Password, user.PasswordHash);

        if(!isPasswordValid) return Unauthorized();


        var token = new LoginResult
        {
            AccessToken = GenerateNewToken(user),
            ExpiresAfter = DateTime.Now.AddHours(2).ToString("yyyy-MM-dd HH':'mm':'ss K")
        };

        var refreshToken = GenerateRefreshToken();

        SetRefreshTokenCookie(refreshToken);

        return Ok(token);
    }

    private async Task<User> FindUserFromRequest(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        if(!string.IsNullOrEmpty(loginRequest.UserName))
            return await _dataContext.Users.Where(user => user.UserName == loginRequest.UserName).FirstOrDefaultAsync(cancellationToken);

        if(!string.IsNullOrEmpty(loginRequest.Email))
            return await _dataContext.Users.Where(user => user.Email == loginRequest.Email).FirstOrDefaultAsync(cancellationToken);

        return null;    
    }

    private string GenerateNewToken(User user)
    {
        var signingCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>() 
        {
            new Claim("preferred_username", user.UserName),
            new Claim("given_name", user.FirstName),
            new Claim("family_name", user.LastName),
            new Claim("phone_number", user.PhoneNumber),
            new Claim("email", user.Email),
            new Claim("role", user.Role.ToString().ToLower()),
            new Claim("email_verified", user.IsEmailConfirmed.ToString())
        };

        var tokeOptions = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
    }

    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            Expires = DateTime.UtcNow.AddDays(2),
            Created = DateTime.UtcNow 
        };

        return refreshToken;
    }

    private void SetRefreshTokenCookie(RefreshToken newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };

        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
    }
}