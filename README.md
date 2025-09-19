This is Assignment for .NET courses and it updates oftens 
BY 
Jwan Hasan

## CLI Application

This project now includes a command-line interface (CLI) application for managing users, posts, and comments.

### Running the CLI

To run the CLI application:

```bash
cd CLI
dotnet run
```

### Available Commands

The CLI supports the following commands:

#### User Management
- `add user` - Add a new user (prompts for username and password)
- `list users` - List all users
- `get user <id>` - Get a specific user by ID
- `update user <id>` - Update a user by ID
- `delete user <id>` - Delete a user by ID

#### Post Management
- `add post` - Add a new post (prompts for title, body, and user ID)
- `list posts` - List all posts
- `get post <id>` - Get a specific post by ID
- `update post <id>` - Update a post by ID
- `delete post <id>` - Delete a post by ID

#### Comment Management
- `add comment` - Add a new comment (prompts for body and user ID)
- `list comments` - List all comments
- `get comment <id>` - Get a specific comment by ID
- `update comment <id>` - Update a comment by ID
- `delete comment <id>` - Delete a comment by ID

#### Utility Commands
- `help` - Show available commands
- `exit` or `quit` - Exit the application

### Example Usage

```
> add user
Enter username: john
Enter password: password123
User added with ID: 1

> list users
Users:
ID      Username
--      --------
1       john

> add post
Enter post title: My First Post
Enter post body: This is my first post content
Enter user ID: 1
Post added with ID: 1

> list posts
Posts:
ID      User ID Title
--      ------- -----
1       1       My First Post
```

### Project Structure

- **Entities**: Contains the domain models (User, Post, Comment)
- **RepositoryContracts**: Contains repository interfaces
- **InMemoryRepositories**: Contains in-memory implementations of repositories
- **CLI**: Console application for interacting with the system