using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository: ICommentRepository
{
    private readonly string filePath = "comments.json";

      public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);

        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 1;

        comment.Id = maxId + 1;

        comments.Add(comment);

        commentsAsJson = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, commentsAsJson);

        return  comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
            

        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        var index = comments.FindIndex(c => c.Id == comment.Id);
        if ( index != -1)
        {
         comments.Insert(index,comment);
        }
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath,commentsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments  = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        var index = comments.FindIndex(c => c.Id == id);
        if (index != -1)
            comments.RemoveAt(index);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments  = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        var index = comments.FindIndex(c => c.Id == id);
        var comment = new Comment();
        if (index != -1)
            comment = comments[index];
        return comment;
    }

    public IQueryable<Comment> GetManyAsync()
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }
}