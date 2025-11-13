using DTOContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using FileRepositories;
using Entities;
using System.Linq;


namespace WebAPI.controller;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UserController(IUserRepository userRepository)
    {
        this.userRepo = userRepository;
    }


    [HttpPost]
    
    
    public async Task<ActionResult<UserDto>> AddUser([FromBody] RequestUserDto request)
    {
        await VerifyUserNameIsAvailableAsync(request.UserName);
        User user = new User{Password = request.Password,UserName = request.UserName};
        User created = await userRepo.AddAsync(user);
        UserDto dto = new()
        {
            Id = created.Id,
            UserName = created.UserName
        };
        return Created($"/user/{dto.Id}", dto);
    }

    private async Task VerifyUserNameIsAvailableAsync(string requestUserName)
    {
        var available = await userRepo.GetUserByUsernameAsync(requestUserName);
        if (available != null)
        {
            throw new Exception($"User already exists {requestUserName}");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] RequestUserDto request)
    {
        var available = await userRepo.GetSingleAsync(id);
 
      if (available==null){ throw new Exception("User not found"); }
      User update = new  User{Id = id, UserName = request.UserName ?? available.UserName};
      
       await userRepo.UpdateAsync(update);
       var dto = new UserDto{Id = update.Id, UserName = update.UserName};
        return Accepted($"/user/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]


    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        var user = await userRepo.GetSingleAsync(id);
        if (user == null) return NotFound();
        return Ok((user));
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllAsync()
    {
       var many= userRepo.GetManyAsync();
       var dtos = many.Select(u => new UserDto{Id = u.Id, UserName = u.UserName}).ToList();
       return Ok(dtos);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await userRepo.DeleteAsync(id);
        return NoContent();
    }
    
    
}