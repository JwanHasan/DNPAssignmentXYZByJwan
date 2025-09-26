
using System.Runtime.InteropServices;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private IUserRepository  userRepository;
    private ICommentRepository commentRepository;
    private IPostRepository postRepository;

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Message();
            string? responce =Console.ReadLine();

            switch (responce)
            {
                case "1":
                {
                    new ManageUsersView(userRepository).ShowAsync(); break;
                }
                case "2":
                {
                    new ManagePostsView(postRepository,commentRepository).ShowAsync();break;
                }
                
                
            }

            if (responce == "0")
            {
              Console.WriteLine("exit program");
                              break;  
            }
                
        }
    }

    private void Message()
    {
        Console.WriteLine($"""
                           Choose command:
                           1 Manage Users
                           2 Manage Posts
                           0 Exit Program
                           
                           """);
    }
}