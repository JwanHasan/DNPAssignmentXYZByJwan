using Entities;

namespace CLI.UI.ManageUsers;

using RepositoryContracts;
public class ManageUsersView
{
    private IUserRepository userRepository;

    public ManageUsersView(IUserRepository userRepositories)
    {
        this.userRepository = userRepositories;
    }

    public void ShowAsync()
    {
       Message();
       string command = Console.ReadLine();
       
       ListUsersView listUsersView = new(userRepository);
       switch (command)
       {
           case "1":
           {

               Console.WriteLine("Please enter userName: ");
               string? userName = Console.ReadLine();

               Console.WriteLine("Please enter Password: ");
               string? password = Console.ReadLine();
               userRepository.AddAsync(new User(){UserName = userName, Password = password});

               break;
           }
               
           case "2":
           {

               listUsersView.Show();
               break;  
           }
           case "3":
           {
               Console.WriteLine("Please enter username id :");
               string s = Console.ReadLine();
               int.TryParse(s, out int id);
               userRepository.DeleteAsync(id);
               break;
           }
       }
    }

    public void Message()
    {
        Console.WriteLine("""
                          Choose option
                          1 Create User
                          2 List Users
                          3 delete User
                          """);
    }
}