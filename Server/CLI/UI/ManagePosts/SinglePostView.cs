using InMemoryRepositories;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private IPostRepository _postRepository;
    private ICommentRepository _commentRepository;
    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }
    public async Task OnePost(int postId)
    {
        var post = await _postRepository.GetSingleAsync(postId);
        var comment = await _commentRepository.GetSingleAsync(postId);
        
        Console.WriteLine($" post:  Title: {post.Title} Body: {post.Body} ID: {post.Id}  UserId: {post.UserId}) \n " +
                          $" comment: PostID:{comment.Id}  UserID: {comment.UserId} Comment: {comment.Body}");
    }
} 
