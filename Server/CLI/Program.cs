// See https://aka.ms/new-console-template for more information
using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;
using Entities;


Console.WriteLine("Starting CLI app... ");
IUserRepository userRepository = new UserInMemoryRepositories();
ICommentRepository commentRepository = new CommentInMemoryRepositories();
IPostRepository postRepository = new PostInMemoryRepositories();

User user = new()
{
    UserName = "name",
    Password = "password",
};
User newUser = new()
{
    UserName = "name1",
    Password = "password1",

};
Post newPost = new(){Body = "Hello World", Title = "how to "};
Comment newComment = new(){Body = "Hello World",Id = newPost.Id,UserId = newUser.Id};

await userRepository.AddAsync(user); 
await userRepository.AddAsync(newUser); 
await commentRepository.AddAsync(newComment);
await postRepository.AddAsync(newPost);
CliApp cliApp = new CliApp( userRepository, commentRepository, postRepository);
await cliApp.StartAsync();