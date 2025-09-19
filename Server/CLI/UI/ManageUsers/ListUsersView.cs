using RepositoryContracts;
using Entities;
namespace CLI.UI.ManageUsers;


public class ListUsersView
{
    private IUserRepository userRepository;
    
    public ListUsersView(IUserRepository userRepository)
    {
       this.userRepository =userRepository;
    }

    public void Show()
    {
        
         userRepository.GetManyAsync().ToList().ForEach(u=> Console.WriteLine($"Username {u.UserName} ID {u.Id}"));
    }
}