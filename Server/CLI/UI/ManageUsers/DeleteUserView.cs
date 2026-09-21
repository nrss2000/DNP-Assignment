using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class DeleteUserView
{
    private readonly IUserRepository userRepository;

    public DeleteUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        int id = ConsoleInput.ReadInt("User id");
        await userRepository.DeleteAsync(id);
        Console.WriteLine("User deleted.");
    }
}
