using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterAPI.Domain.Requests;
using RegisterAPI.Infrastructure;

namespace user_api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserGroupController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly DataContext _dataContext;

    public UserGroupController(ILogger<UsersController> logger, DataContext dataContext)
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
        var userGroups = await _dataContext.UserGroups.ToListAsync(cancellationToken);

        if(userGroups is null || !userGroups.Any())
            return NoContent();

        return Ok(userGroups);
    }

    [HttpGet("{userGroupId}"), Authorize(Roles = "user, admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Get([FromRoute] Guid userGroupId, CancellationToken cancellationToken)
    {
        var userGroup = await _dataContext.UserGroups.FindAsync(userGroupId);
        if (userGroup is null)
            return NotFound($"Requested user with id {userGroupId} not found");

        return Ok(userGroup);
    }

    [HttpPost, Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Post([FromBody] CreateUserGroupRequest createUserGroupRequest, CancellationToken cancellationToken)
    {
        var userGroup = createUserGroupRequest.Create();
        _dataContext.UserGroups.Add(userGroup);
        await _dataContext.SaveChangesAsync(cancellationToken);

        var url = $"{Request.Scheme}://{Request.Host.Value}/users/{userGroup.Id}";

        return Created(url, userGroup);
    }

    [HttpPut("{userGroupId}"), Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Put([FromRoute] Guid userGroupId, [FromBody] UpdateUserGroupRequest updateUserGroupRequest, CancellationToken cancellationToken)
    {
        var userGroup = await _dataContext.UserGroups.FindAsync(userGroupId);
        if (userGroup is null)
            return NotFound($"Requested user group with id {userGroupId} not found");

        var updatedUserGroup = updateUserGroupRequest.Update(userGroup);
        _dataContext.UserGroups.Update(updatedUserGroup);
        await _dataContext.SaveChangesAsync();

        return Ok(userGroup);
    }

    [HttpDelete("{userGroupId}"), Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] Guid userGroupId, CancellationToken cancellationToken)
    {
        var userGroup = await _dataContext.UserGroups.FindAsync(userGroupId);
        if (userGroup is null)
            return NotFound($"Requested user group with id {userGroupId} not found");

        _dataContext.Remove(userGroup);
        await _dataContext.SaveChangesAsync();

        return NoContent();
    }
}