using Entities;
using InMemoryRepositories;
using RepositoryContracts;

namespace CLI;

class Program
{
    private static IUserRepository userRepository = new UserInMemoryRepositories();
    private static IPostRepository postRepository = new PostInMemoryRepositories();
    private static ICommentRepository commentRepository = new CommentInMemoryRepositories();

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== DNP Assignment CLI ===");
        Console.WriteLine("Available commands:");
        PrintHelp();

        while (true)
        {
            Console.Write("\n> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                continue;

            var command = parts[0].ToLower();
            var entity = parts.Length > 1 ? parts[1].ToLower() : "";

            try
            {
                switch (command)
                {
                    case "help":
                        PrintHelp();
                        break;
                    case "exit":
                    case "quit":
                        Console.WriteLine("Goodbye!");
                        return;
                    case "add":
                        await HandleAdd(entity, parts);
                        break;
                    case "list":
                        await HandleList(entity);
                        break;
                    case "get":
                        await HandleGet(entity, parts);
                        break;
                    case "update":
                        await HandleUpdate(entity, parts);
                        break;
                    case "delete":
                        await HandleDelete(entity, parts);
                        break;
                    default:
                        Console.WriteLine($"Unknown command: {command}. Type 'help' for available commands.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("\nCommands:");
        Console.WriteLine("  help                    - Show this help");
        Console.WriteLine("  exit/quit               - Exit the application");
        Console.WriteLine("  add user                - Add a new user");
        Console.WriteLine("  add post                - Add a new post");
        Console.WriteLine("  add comment             - Add a new comment");
        Console.WriteLine("  list users              - List all users");
        Console.WriteLine("  list posts              - List all posts");
        Console.WriteLine("  list comments           - List all comments");
        Console.WriteLine("  get user <id>           - Get user by ID");
        Console.WriteLine("  get post <id>           - Get post by ID");
        Console.WriteLine("  get comment <id>        - Get comment by ID");
        Console.WriteLine("  update user <id>        - Update user by ID");
        Console.WriteLine("  update post <id>        - Update post by ID");
        Console.WriteLine("  update comment <id>     - Update comment by ID");
        Console.WriteLine("  delete user <id>        - Delete user by ID");
        Console.WriteLine("  delete post <id>        - Delete post by ID");
        Console.WriteLine("  delete comment <id>     - Delete comment by ID");
    }

    static async Task HandleAdd(string entity, string[] parts)
    {
        switch (entity)
        {
            case "user":
                await AddUser();
                break;
            case "post":
                await AddPost();
                break;
            case "comment":
                await AddComment();
                break;
            default:
                Console.WriteLine("Specify what to add: user, post, or comment");
                break;
        }
    }

    static async Task HandleList(string entity)
    {
        switch (entity)
        {
            case "users":
                await ListUsers();
                break;
            case "posts":
                await ListPosts();
                break;
            case "comments":
                await ListComments();
                break;
            default:
                Console.WriteLine("Specify what to list: users, posts, or comments");
                break;
        }
    }

    static async Task HandleGet(string entity, string[] parts)
    {
        if (parts.Length < 3 || !int.TryParse(parts[2], out int id))
        {
            Console.WriteLine("Please provide a valid ID number");
            return;
        }

        switch (entity)
        {
            case "user":
                await GetUser(id);
                break;
            case "post":
                await GetPost(id);
                break;
            case "comment":
                await GetComment(id);
                break;
            default:
                Console.WriteLine("Specify what to get: user, post, or comment");
                break;
        }
    }

    static async Task HandleUpdate(string entity, string[] parts)
    {
        if (parts.Length < 3 || !int.TryParse(parts[2], out int id))
        {
            Console.WriteLine("Please provide a valid ID number");
            return;
        }

        switch (entity)
        {
            case "user":
                await UpdateUser(id);
                break;
            case "post":
                await UpdatePost(id);
                break;
            case "comment":
                await UpdateComment(id);
                break;
            default:
                Console.WriteLine("Specify what to update: user, post, or comment");
                break;
        }
    }

    static async Task HandleDelete(string entity, string[] parts)
    {
        if (parts.Length < 3 || !int.TryParse(parts[2], out int id))
        {
            Console.WriteLine("Please provide a valid ID number");
            return;
        }

        switch (entity)
        {
            case "user":
                await DeleteUser(id);
                break;
            case "post":
                await DeletePost(id);
                break;
            case "comment":
                await DeleteComment(id);
                break;
            default:
                Console.WriteLine("Specify what to delete: user, post, or comment");
                break;
        }
    }

    // User operations
    static async Task AddUser()
    {
        Console.Write("Enter username: ");
        var username = Console.ReadLine();
        Console.Write("Enter password: ");
        var password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username and password cannot be empty");
            return;
        }

        var user = new User
        {
            UserName = username,
            Password = password
        };

        var addedUser = await userRepository.AddAsync(user);
        Console.WriteLine($"User added with ID: {addedUser.Id}");
    }

    static async Task ListUsers()
    {
        var users = userRepository.GetManyAsync().ToList();
        if (!users.Any())
        {
            Console.WriteLine("No users found");
            return;
        }

        Console.WriteLine("\nUsers:");
        Console.WriteLine("ID\tUsername");
        Console.WriteLine("--\t--------");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}\t{user.UserName}");
        }
    }

    static async Task GetUser(int id)
    {
        var user = await userRepository.GetSingleAsync(id);
        Console.WriteLine($"User ID: {user.Id}");
        Console.WriteLine($"Username: {user.UserName}");
        Console.WriteLine($"Password: [HIDDEN]");
    }

    static async Task UpdateUser(int id)
    {
        var user = await userRepository.GetSingleAsync(id);
        Console.WriteLine($"Current username: {user.UserName}");
        
        Console.Write("Enter new username (or press Enter to keep current): ");
        var newUsername = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newUsername))
            user.UserName = newUsername;

        Console.Write("Enter new password (or press Enter to keep current): ");
        var newPassword = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newPassword))
            user.Password = newPassword;

        await userRepository.UpdateAsync(user);
        Console.WriteLine("User updated successfully");
    }

    static async Task DeleteUser(int id)
    {
        await userRepository.DeleteAsync(id);
        Console.WriteLine($"User with ID {id} deleted successfully");
    }

    // Post operations
    static async Task AddPost()
    {
        Console.Write("Enter post title: ");
        var title = Console.ReadLine();
        Console.Write("Enter post body: ");
        var body = Console.ReadLine();
        Console.Write("Enter user ID: ");
        
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID");
            return;
        }

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Title and body cannot be empty");
            return;
        }

        var post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        var addedPost = await postRepository.AddAsync(post);
        Console.WriteLine($"Post added with ID: {addedPost.Id}");
    }

    static async Task ListPosts()
    {
        var posts = postRepository.GetManyAsync().ToList();
        if (!posts.Any())
        {
            Console.WriteLine("No posts found");
            return;
        }

        Console.WriteLine("\nPosts:");
        Console.WriteLine("ID\tUser ID\tTitle");
        Console.WriteLine("--\t-------\t-----");
        foreach (var post in posts)
        {
            var shortTitle = post.Title.Length > 30 ? post.Title.Substring(0, 30) + "..." : post.Title;
            Console.WriteLine($"{post.Id}\t{post.UserId}\t{shortTitle}");
        }
    }

    static async Task GetPost(int id)
    {
        var post = await postRepository.GetSingleAsync(id);
        Console.WriteLine($"Post ID: {post.Id}");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine($"User ID: {post.UserId}");
    }

    static async Task UpdatePost(int id)
    {
        var post = await postRepository.GetSingleAsync(id);
        Console.WriteLine($"Current title: {post.Title}");
        Console.WriteLine($"Current body: {post.Body}");
        
        Console.Write("Enter new title (or press Enter to keep current): ");
        var newTitle = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newTitle))
            post.Title = newTitle;

        Console.Write("Enter new body (or press Enter to keep current): ");
        var newBody = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newBody))
            post.Body = newBody;

        Console.Write("Enter new user ID (or press Enter to keep current): ");
        var userIdInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(userIdInput) && int.TryParse(userIdInput, out int newUserId))
            post.UserId = newUserId;

        await postRepository.UpdateAsync(post);
        Console.WriteLine("Post updated successfully");
    }

    static async Task DeletePost(int id)
    {
        await postRepository.DeleteAsync(id);
        Console.WriteLine($"Post with ID {id} deleted successfully");
    }

    // Comment operations
    static async Task AddComment()
    {
        Console.Write("Enter comment body (as number - note: this is a data model limitation): ");
        if (!int.TryParse(Console.ReadLine(), out int body))
        {
            Console.WriteLine("Invalid body (must be a number due to current data model)");
            return;
        }

        Console.Write("Enter user ID: ");
        var userId = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userId))
        {
            Console.WriteLine("User ID cannot be empty");
            return;
        }

        var comment = new Comment
        {
            Body = body,
            UserId = userId
        };

        var addedComment = await commentRepository.AddAsync(comment);
        Console.WriteLine($"Comment added with ID: {addedComment.Id}");
    }

    static async Task ListComments()
    {
        var comments = commentRepository.GetManyAsync().ToList();
        if (!comments.Any())
        {
            Console.WriteLine("No comments found");
            return;
        }

        Console.WriteLine("\nComments:");
        Console.WriteLine("ID\tUser ID\tBody");
        Console.WriteLine("--\t-------\t----");
        foreach (var comment in comments)
        {
            Console.WriteLine($"{comment.Id}\t{comment.UserId}\t{comment.Body}");
        }
    }

    static async Task GetComment(int id)
    {
        var comment = await commentRepository.GetSingleAsync(id);
        Console.WriteLine($"Comment ID: {comment.Id}");
        Console.WriteLine($"Body: {comment.Body}");
        Console.WriteLine($"User ID: {comment.UserId}");
    }

    static async Task UpdateComment(int id)
    {
        var comment = await commentRepository.GetSingleAsync(id);
        Console.WriteLine($"Current body: {comment.Body}");
        Console.WriteLine($"Current user ID: {comment.UserId}");
        
        Console.Write("Enter new body (or press Enter to keep current): ");
        var bodyInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(bodyInput) && int.TryParse(bodyInput, out int newBody))
            comment.Body = newBody;

        Console.Write("Enter new user ID (or press Enter to keep current): ");
        var newUserId = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newUserId))
            comment.UserId = newUserId;

        await commentRepository.UpdateAsync(comment);
        Console.WriteLine("Comment updated successfully");
    }

    static async Task DeleteComment(int id)
    {
        await commentRepository.DeleteAsync(id);
        Console.WriteLine($"Comment with ID {id} deleted successfully");
    }
}
