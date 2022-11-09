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
    public async Task<IActionResult> GetAll()
    {
        var users = await _dataContext.Users.ToListAsync();

        if(users is null || !users.Any())
            return NoContent();

        return Ok(users);
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid userId)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user is null)
            return NotFound($"Requested user with id {userId} not found");

        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] CreateUserRequest createUserRequest)
    {
        var user = createUserRequest.Create();
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        var url = $"{Request.Scheme}://{Request.Host.Value}/users/{user.Id}";

        return Created(url, user);
    }

    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromRoute] Guid userId, [FromBody] UpdateUserRequest updateUserRequest)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user is null)
            return NotFound($"Requested user with id {userId} not found");

        var updatedUser = updateUserRequest.Update(user);
        _dataContext.Users.Update(updatedUser);
        await _dataContext.SaveChangesAsync();

        return Ok(user);
    }

    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user is null)
            return NotFound($"Requested user with id {userId} not found");

        _dataContext.Remove(user);
        await _dataContext.SaveChangesAsync();

        return NoContent();
    }
}