using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class UserInMemoryRepositories : IUserRepository
{
    public List<User> Users = new();

    public Task<User> AddAsync(User user)
    {
        user.Id = Users.Any()
            ? Users.Max(p => p.Id) + 1
            : 1;
        Users.Add(user);
        return Task.FromResult(user);

    } 
    public Task UpdateAsync(User user)
    {
        User? existingUser = Users.SingleOrDefault(p => p.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        Users.Remove(existingUser);
        Users.Add(user);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        User? userToRemove = Users.SingleOrDefault(p => p.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        Users.Remove(userToRemove);
        return Task.CompletedTask;
    }
    
    public Task<User> GetSingleAsync(int id)
    {
        User? getSingle = Users.SingleOrDefault(p => p.Id == id);
        if (getSingle is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        return Task.FromResult(getSingle);
    }
    public IQueryable<User> GetManyAsync()
    {
        return Users.AsQueryable();
    }


}