using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        try
        {
            bool usernameTaken = userRepository.GetMany().Any(u => u.Username == dto.Username);
            if (usernameTaken)
            {
                return BadRequest($"Username '{dto.Username}' is already taken");
            }

            User user = new User
            {
                Username = dto.Username,
                Password = dto.Password
            };
            User added = await userRepository.AddAsync(user);
            UserDto userDto = new UserDto
            {
                Id = added.Id,
                Username = added.Username
            };
            return Created($"/users/{userDto.Id}", userDto);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] string? username)
    {
        try
        {
            IQueryable<User> users = userRepository.GetMany();

            if (!string.IsNullOrWhiteSpace(username))
            {
                users = users.Where(u => u.Username.Contains(username));
            }

            List<UserDto> userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username
            }).ToList();

            return Ok(userDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetSingleUser(int id)
    {
        try
        {
            User user = await userRepository.GetSingleAsync(id);
            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };
            return Ok(userDto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, UpdateUserDto dto)
    {
        try
        {
            User user = new User
            {
                Id = id,
                Username = dto.Username,
                Password = dto.Password
            };
            await userRepository.UpdateAsync(user);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
