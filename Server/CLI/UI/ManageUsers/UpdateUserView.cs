using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class UpdateUserView
{
    private readonly IUserRepository userRepository;

    public UpdateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        int id = ConsoleInput.ReadInt("User id");
        User existing = await userRepository.GetSingleAsync(id);

        string? username = ConsoleInput.ReadOptional($"New username (empty keeps '{existing.Username}')");
        string? password = ConsoleInput.ReadOptional("New password (empty keeps current)");

        if (username is not null && userRepository.GetMany()
                .Any(u => u.Id != id && u.Username.ToLower() == username.ToLower()))
        {
            Console.WriteLine("That username is already taken.");
            return;
        }

        await userRepository.UpdateAsync(new User
        {
            Id = existing.Id,
            Username = username ?? existing.Username,
            Password = password ?? existing.Password
        });
        Console.WriteLine("User updated.");
    }
}
