using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterAPI.Domain;
using RegisterAPI.Domain.Requests;
using RegisterAPI.Infrastructure;
using Scrypt;

namespace user_api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly DataContext _dataContext;

    public AuthenticationController(ILogger<UsersController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await FindUserFromRequest(loginRequest);

        if(user is null)
            return NotFound("User not found");

        var isPasswordValid = new ScryptEncoder().Compare(user.PasswordSalt + loginRequest.Password, user.PasswordHash);

        if(isPasswordValid)
            return Ok();
        else
            return Unauthorized();
    }

    private async Task<User> FindUserFromRequest(LoginRequest loginRequest)
    {
        if(!string.IsNullOrEmpty(loginRequest.UserName))
            return await _dataContext.Users.Where(user => user.UserName == loginRequest.UserName).FirstOrDefaultAsync();

        if(!string.IsNullOrEmpty(loginRequest.Email))
            return await _dataContext.Users.Where(user => user.Email == loginRequest.Email).FirstOrDefaultAsync();

        return null;    
    }
}