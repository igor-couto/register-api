using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterAPI.Domain.Requests;
using RegisterAPI.Infrastructure;

namespace user_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly DataContext _dataContext;

    public UsersController(ILogger<UsersController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    [HttpGet()]
    public async Task<IResult> GetAll()
    {
        return Results.Ok();
    }

    [HttpGet("{userId}/")]
    public async Task<IResult> Get([FromRoute] string userId)
    {
        var users = await _dataContext.Users.ToListAsync();
        return Results.Ok(users);
    }

    [HttpPost()]
    public async Task<IResult> Post([FromBody] CreateUserRequest createUserRequest)
    {
        return Results.Ok();
    }

    [HttpPut("{userId}/")]
    public async Task<IResult> Put([FromRoute] UpdateUserRequest updateUserRequest)
    {
        return Results.Ok();
    }

    [HttpDelete("{userId}/")]
    public async Task<IResult> Delete([FromRoute] string userId)
    {
        return Results.Ok();
    }
}
