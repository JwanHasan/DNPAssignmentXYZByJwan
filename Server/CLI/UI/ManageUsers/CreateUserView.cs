using Entities;
using FileRepositories;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepositories)
    {
        this.userRepository = userRepositories;
    }

    public async Task CreateUser(string userName, string password)
    {
        await userRepository.AddAsync(new User(){UserName = userName, Password = password});
    }
}