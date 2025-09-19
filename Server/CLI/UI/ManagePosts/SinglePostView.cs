using InMemoryRepositories;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private IPostRepository _postRepository;
    private ICommentRepository _commentRepository;
    public SinglePostView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
        _commentRepository = new CommentInMemoryRepositories();
    }
    public void OnePost(int postId)
    {
        
        Console.WriteLine($" post: {_postRepository.GetSingleAsync(postId).Id}) \n " +
                          $" comment: {_commentRepository.GetSingleAsync(postId).Id}");
    }
}
