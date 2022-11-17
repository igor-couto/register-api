using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scrypt;
using RegisterAPI.Domain;
using RegisterAPI.Domain.Requests;
using RegisterAPI.Infrastructure;

namespace user_api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UsersController> _logger;
    private readonly DataContext _dataContext;

    public AuthenticationController(IConfiguration configuration, ILogger<UsersController> logger, DataContext dataContext)
    {
        _configuration = configuration;
        _logger = logger;
        _dataContext = dataContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var user = await FindUserFromRequest(loginRequest, cancellationToken);

        if(user is null)
            return NotFound("User not found");

        var isPasswordValid = new ScryptEncoder().Compare(user.PasswordSalt + loginRequest.Password, user.PasswordHash);

        if(isPasswordValid)
            return Ok(GenerateNewToken(user));
        else
            return Unauthorized();
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
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>() 
        {
            new Claim("preferred_username", user.UserName),
            new Claim("given_name", user.FirstName),
            new Claim("family_name", user.LastName),
            new Claim("phone_number", user.PhoneNumber),
            new Claim("email", user.Email),
            new Claim("role", "user"),
            new Claim("email_verified", user.IsEmailConfirmed.ToString())
        };

        var tokeOptions = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
    }
}