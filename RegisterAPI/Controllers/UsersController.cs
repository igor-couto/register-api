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

    [HttpGet]
    public async Task<IResult> GetAll()
    {
        var users = await _dataContext.Users.ToListAsync();
        return Results.Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<IResult> Get([FromRoute] Guid userId)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user is null)
            return Results.NotFound($"Requested user with id {userId} not found");

        return Results.Ok(user);
    }

    [HttpPost]
    public async Task<IResult> Post([FromBody] CreateUserRequest createUserRequest)
    {
        var user = createUserRequest.Create();
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        return Results.Created($"users/{user.Id}", user);
    }

    [HttpPut("{userId}")]
    public async Task<IResult> Put([FromRoute] Guid userId, [FromBody] UpdateUserRequest updateUserRequest)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user is null)
            return Results.NotFound($"Requested user with id {userId} not found");

        var updatedUser = updateUserRequest.Update(user);
        _dataContext.Users.Update(updatedUser);
        await _dataContext.SaveChangesAsync();

        return Results.Ok(user);
    }

    [HttpDelete("{userId}")]
    public async Task<IResult> Delete([FromRoute] Guid userId)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user is null)
            return Results.NotFound($"Requested user with id {userId} not found");

        _dataContext.Remove(user);
        await _dataContext.SaveChangesAsync();

        return Results.Ok();
    }
}