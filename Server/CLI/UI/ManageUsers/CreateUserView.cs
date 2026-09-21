using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        string username = ConsoleInput.ReadRequired("Username");
        if (userRepository.GetMany().Any(u => u.Username.ToLower() == username.ToLower()))
        {
            Console.WriteLine("That username is already taken.");
            return;
        }
        string password = ConsoleInput.ReadRequired("Password");

        User created = await userRepository.AddAsync(new User { Username = username, Password = password });
        Console.WriteLine($"Created user '{created.Username}' with id {created.Id}.");
    }
}
