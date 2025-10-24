using DTOContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using FileRepositories;
using Entities;


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
        return Created($"/user/{dto.Id}", created);
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
    public async Task<ActionResult<UserDto>> Update([FromBody] int id, [FromBody] RequestUserDto request)
    {
        var available = await userRepo.GetUserByUsernameAsync(request.UserName);
 
      if (request.UserName==null){ throw new Exception("User not found"); }
      User update = new  User{Id = id, UserName = request.UserName};
      
       await userRepo.UpdateAsync(update);
        return Accepted( update);
    }

    [HttpGet("{id:int}")]


    public async Task<ActionResult<UserDto>> GetSingle([FromBody]int id)
    {
        var user = await userRepo.GetSingleAsync(id);
        if (user == null) return NotFound();
        return Ok((user));
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllAsync()
    {
       var many=  userRepo.GetManyAsync();
       return Ok(many);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromBody]int id)
    {
        await userRepo.DeleteAsync(id);
        return NoContent();
    }
    
    
}