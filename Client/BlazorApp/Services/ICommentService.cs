using DTOContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddAsync(RequestCommentDto comment);
    Task UpdateAsync(RequestCommentDto comment);
    Task DeleteAsync(int id);
    Task<CommentDto> GetSingleAsync(int id);
    IQueryable<CommentDto> GetManyAsync();  
}