using Entities;
using InMemoryRepositories;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private IPostRepository postRepositories;
    public ManagePostsView(IPostRepository postRepository)
    {
        this.postRepositories = postRepository;
    }

    public void ShowAsync()
    {
        Message();
        string? commands = Console.ReadLine();
        ListPostsView postList = new(postRepositories);
        SinglePostView singlePostView = new(postRepositories);
        switch (commands)
        {
            case "1":
            {
                Console.WriteLine("Please enter your userID : ");
                string? ud = Console.ReadLine();
                int.TryParse(ud,out int userId);
                
                Console.WriteLine("Please enter Title: ");
                string? title = Console.ReadLine();
                
                Console.WriteLine("Please enter Content: ");
                string? body = Console.ReadLine();
                
                postRepositories.AddAsync(new Post() { UserId =userId,Title = title, Body = body });
                break;
            }
            case "2":
            {
                postList.ListPosts();
                Console.WriteLine("Please select post id to comment on it : ");
                string? comment = Console.ReadLine();
                int.TryParse(comment,out int postId);
                
                Console.WriteLine("Please enter UserID : ");
                string? userId = Console.ReadLine();
                int.TryParse(userId,out int usId);
                
                Console.WriteLine("Write the comment");
                string? commentBody = Console.ReadLine();
                ICommentRepository comnts = new CommentInMemoryRepositories();
                comnts.AddAsync(new Comment() { Body = commentBody, Id = postId, UserId = usId});
                break;
            }
            case "3":
            {
                Console.WriteLine("Please enter Post Id : ");
                string? p = Console.ReadLine();
                int.TryParse(p, out int postId);
                singlePostView.OnePost(postId);
                break;
            }
            case "4":
            {
                postList.ListPosts();
                break;
            }
            case "5":
            {
                Console.WriteLine("Please enter Post Id to delete  : ");
                string? dp = Console.ReadLine();
                int.TryParse(dp,out int postId);
                postRepositories.DeleteAsync(postId);
                break;
            }
                
        }
    }

    public void Message()
    {
        Console.WriteLine("""
                          Welcome to the Manage Posts Select your task.
                          1 Creates a new Post
                          2 Creates a new Comment
                          3 View a Post
                          4 View Posts
                          5 Delete a Post
                          
                          """);
    }
}