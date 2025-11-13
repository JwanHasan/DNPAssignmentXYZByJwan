using DTOContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;

namespace WebAPI.controller;
[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
   private readonly ICommentRepository _commentRepo;

    public CommentController(ICommentRepository commentRepository)
    {
        this._commentRepo = commentRepository;
    }


    [HttpPost]
    
    
    public async Task<ActionResult<CommentDto>> AddPost([FromBody] RequestCommentDto request)
    {
        await VerifyCommentIdIsAvailableAsync(request.Id);
        Comment comment = new Comment{Id = request.Id,Body = request.Body,UserId = request.UserId};
        Comment created = await _commentRepo.AddAsync(comment);
        CommentDto dto = new()
        {
            Id = created.Id,
            Body = created.Body,
            UserId = created.UserId
        };
        return Created($"/Comment/{dto.Id}", dto);
    }

    private async Task VerifyCommentIdIsAvailableAsync(int requestId)
    {
        var available = await _commentRepo.GetSingleAsync(requestId);
        if (available != null)
        {
            throw new Exception($"Comment already exists {requestId}");
        }
    }

    
        
    

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> UpdateComment(int id, [FromBody] RequestCommentDto request)
    {
        var available = await _commentRepo.GetSingleAsync(id);
 
      if (available==null){ throw new Exception("Comment not found"); }
      Comment update = new  Comment{Id = id, Body = request.Body ?? available.Body, UserId = request.UserId == 0 ? available.UserId : request.UserId};
      
       await _commentRepo.UpdateAsync(update);
       var dto = new CommentDto{Id = update.Id, Body = update.Body, UserId = update.UserId};
        return Accepted($"/Comment/{dto.Id}", dto);
    }

    [HttpGet("{id:int}")]


    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        var comment = await _commentRepo.GetSingleAsync(id);
        if (comment == null)
        {
            throw new Exception("Post not found");
        }
        var dto = new CommentDto{Id = comment.Id, Body = comment.Body, UserId = comment.UserId};
        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentDto>>> GetAllAsync()
    {
       var many= _commentRepo.GetManyAsync();
       var dtos = many.Select(c => new CommentDto{Id = c.Id, Body = c.Body, UserId = c.UserId}).ToList();
       return Ok(dtos);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _commentRepo.DeleteAsync(id);
        return NoContent();
    }
    
     
}