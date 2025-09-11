using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class CommentInMemoryRepositories
{
    public List<Comment> Comments = new();
    
        public Task<Comment> AddAsync(Comment comment)
        {
            comment.Id = Comments.Any()
                ? Comments.Max(c Comment.Id) + 1
                : 1;
            Comments.Add(comment);
            return Task.FromResult(comment);
    
        } 
        public Task UpdateAsync(Comment comment)
        {
            Comment? existingComment = Comments.SingleOrDefault(c => c.Id == comment.Id);
            if (existingComment is null)
            {
                throw new InvalidOperationException(
                    $"Comment with ID '{comment.Id}' not found");
            }
    
            Comments.Remove(existingComment);
            Comments.Add(comment);
    
            return Task.CompletedTask;
        }
        
        public Task DeleteAsync(int id)
        {
            Comment? commentToRemove = Comments.SingleOrDefault(c Comment.Id == id);
            if (commentToRemove is null)
            {
                throw new InvalidOperationException(
                    $"Comment with ID '{id}' not found");
            }
    
            Comments.Remove(commentToRemove);
            return Task.CompletedTask;
        }
        
        public Task<Comment> GetSingleAsync(int id)
        {
            Comment? getSingle = Comments.SingleOrDefault(c => c.Id == id);
            if (getSingle is null)
            {
                throw new InvalidOperationException(
                    $"Comment with ID '{id}' not found");
            }
            return Task.FromResult(getSingle);
        }
        public IQueryable<Comment> GetManyAsync()
        {
            return Comments.AsQueryable();
        }
    

}