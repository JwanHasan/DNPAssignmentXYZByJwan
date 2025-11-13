using DTOContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddAsync(RequestUserDto user);
    Task UpdateAsync(RequestUserDto user);
    Task DeleteAsync(string username);
    Task<UserDto> GetSingleAsync(string username);
    IQueryable<UserDto> GetManyAsync();
}