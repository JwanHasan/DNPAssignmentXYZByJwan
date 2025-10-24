using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository:IUserRepository
{
    private readonly string filePath = "user.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
            File.WriteAllText(filePath, "[]");
    }

    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);

        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        int maxId = users.Count > 0 ? users.Max(u => u.Id) : 1;

        user.Id = maxId + 1;

        users.Add(user);

        usersAsJson = JsonSerializer.Serialize(users);

        await File.WriteAllTextAsync(filePath, usersAsJson);

        return  user;
    }

    public async Task UpdateAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
            

        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        var index = users.FindIndex(u => u.Id == user.Id);
        if ( index == -1)
        {
            throw new Exception("User not found");
        }
        users[index] = user;
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath,usersAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        var user = users.FirstOrDefault(u => u.Id == id);
        if ( user == null)
        {
            throw new Exception("Post not found");
        }
        users.Remove(user);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath,usersAsJson);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        
        User? user= users.FirstOrDefault(u => u.Id == id);
        if ( user == null )
        {
            throw new Exception("User not found");
        }
        return user;
    }

    public  IQueryable<User> GetManyAsync()
    {
        string usersAsJson =  File.ReadAllTextAsync(filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        string userAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson)!;

        User? user = users.FirstOrDefault(u => u.UserName == username);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }
}