using DTOContracts;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.controller;
[ApiController]
[Route("[controller]")]
public class PostController
{
    private readonly IPostRepository postRepo;

    public PostController(IPostRepository postRepository)
    {
        this.postRepo = postRepository;
    }


    [HttpPost]
    
    
    public async Task<ActionResult<UserDto>> AddPost([FromBody] RequestPostDto request)
    {
        await VerifyPostIdIsAvailableAsync(request.Id);
        Post post = new Post{Id = request.Id,Title = request.Title, Body = request.Body,UserId = request.UserId};
        Post created = await postRepo.AddAsync(post);
        PostDto dto = new()
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            UserId = created.UserId
        };
        return OkResult($"/post/{dto.Id}", created);
    }

    private async Task VerifyPostIdIsAvailableAsync(int requestId)
    {
        var available = await postRepo.GetSingleAsync(requestId);
        if (available != null)
        {
            throw new Exception($"Post already exists {requestId}");
        }
    }

    
        
    

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> UpdatePost([FromBody] int id )
    {
        var available = await postRepo.GetSingleAsync(id);
 
      if (available==null){ throw new Exception("Post not found"); }
      Post update = new  Post{Id = id, Title = available.Title, Body = available.Body, UserId = available.UserId};
      
       await postRepo.UpdateAsync(update);
        return Accepted( update);
    }

    [HttpGet("{id:int}")]


    public async Task<ActionResult<UserDto>> GetSingle([FromBody]int id)
    {
        var post = await postRepo.GetSingleAsync(id);
        if (post == null)
        {
            throw new Exception("Post not found");
        }
        return Ok((post));
    }

    [HttpGet]
    public  Task<ActionResult<List<UserDto>>> GetAllAsync()
    {
       var many=  postRepo.GetManyAsync();
       return Ok(many);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromBody]int id)
    {
        await postRepo.DeleteAsync(id);
        return Ok();
    }
    
    
}