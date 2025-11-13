using DTOContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;

namespace WebAPI.controller;
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository postRepo;

    public PostController(IPostRepository postRepository)
    {
        this.postRepo = postRepository;
    }


    [HttpPost]
    
    
    public async Task<ActionResult<PostDto>> AddPost([FromBody] RequestPostDto request)
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
        return Created($"/post/{dto.Id}", dto);
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
    public async Task<ActionResult<PostDto>> UpdatePost(int id, [FromBody] RequestPostDto request)
    {
        var available = await postRepo.GetSingleAsync(id);
 
      if (available==null){ throw new Exception("Post not found"); }
      Post update = new  Post{Id = id, Title = request.Title ?? available.Title, Body = request.Body ?? available.Body, UserId = request.UserId == 0 ? available.UserId : request.UserId};
      
       await postRepo.UpdateAsync(update);
       var dto = new PostDto{Id = update.Id, Title = update.Title, Body = update.Body, UserId = update.UserId};
        return Accepted($"/post/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]


    public async Task<ActionResult<PostDto>> GetSingle(int id)
    {
        var post = await postRepo.GetSingleAsync(id);
        if (post == null)
        {
            throw new Exception("Post not found");
        }
        var dto = new PostDto{Id = post.Id, Title = post.Title, Body = post.Body, UserId = post.UserId};
        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDto>>> GetAllAsync()
    {
       var many= postRepo.GetManyAsync();
       var dtos = many.Select(p => new PostDto{Id = p.Id, Title = p.Title, Body = p.Body, UserId = p.UserId}).ToList();
       return Ok(dtos);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await postRepo.DeleteAsync(id);
        return NoContent();
    }
    
    
}