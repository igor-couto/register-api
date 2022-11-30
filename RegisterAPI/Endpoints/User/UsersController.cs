using Microsoft.AspNetCore.Authorization;
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

    [HttpGet, Authorize(Roles = "user, admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var users = await _dataContext.Users.ToListAsync(cancellationToken);

        if(users is null || !users.Any())
            return NoContent();

        return Ok(users);
    }

    [HttpGet("{userId}"), Authorize(Roles = "user, admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Get([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user is null)
            return NotFound($"Requested user with id {userId} not found");

        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] CreateUserRequest createUserRequest, CancellationToken cancellationToken)
    {
        var user = createUserRequest.Create();
        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync(cancellationToken);

        var url = $"{Request.Scheme}://{Request.Host.Value}/users/{user.Id}";

        return Created(url, user);
    }

    [HttpPut("{userId}"), Authorize(Roles = "user, admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Put([FromRoute] Guid userId, [FromBody] UpdateUserRequest updateUserRequest, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user is null)
            return NotFound($"Requested user with id {userId} not found");

        var updatedUser = updateUserRequest.Update(user);
        _dataContext.Users.Update(updatedUser);
        await _dataContext.SaveChangesAsync();

        return Ok(user);
    }

    [HttpDelete("{userId}"), Authorize(Roles = "user, admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user is null)
            return NotFound($"Requested user with id {userId} not found");

        _dataContext.Remove(user);
        await _dataContext.SaveChangesAsync();

        return NoContent();
    }
}