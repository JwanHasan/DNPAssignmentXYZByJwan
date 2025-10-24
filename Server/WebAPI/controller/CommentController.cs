using DTOContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.controller;
[ApiController]
[Route("[controller]")]
public class CommentController
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
        return OkResult($"/Comment/{dto.Id}", created);
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
    public async Task<ActionResult<CommentDto>> UpdateComment([FromBody] int id )
    {
        var available = await _commentRepo.GetSingleAsync(id);
 
      if (available==null){ throw new Exception("Comment not found"); }
      Comment update = new  Comment{Id = id, Body = available.Body, UserId = available.UserId};
      
       await _commentRepo.UpdateAsync(update);
        return Accepted( update);
    }

    [HttpGet("{id:int}")]


    public async Task<ActionResult<CommentDto>> GetSingle([FromBody]int id)
    {
        var comment = await _commentRepo.GetSingleAsync(id);
        if (comment == null)
        {
            throw new Exception("Post not found");
        }
        return Ok((comment));
    }

    [HttpGet]
    public  Task<ActionResult<List<CommentDto>>> GetAllAsync()
    {
       var many=  _commentRepo.GetManyAsync();
       return Ok(many);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromBody]int id)
    {
        await _commentRepo.DeleteAsync(id);
        return Ok();
    }
    
     
}