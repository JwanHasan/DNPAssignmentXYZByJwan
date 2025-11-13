using DTOContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddAsync(RequestPostDto postDto);
    Task UpdateAsync(RequestPostDto postDto);
    Task DeleteAsync(int postId);
    Task<PostDto> GetSingleAsync(int postId);
    IQueryable<PostDto> GetManyAsync();
}