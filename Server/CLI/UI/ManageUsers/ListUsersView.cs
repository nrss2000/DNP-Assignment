using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public Task ShowAsync()
    {
        string? word = ConsoleInput.ReadOptional("Username contains (leave empty for all)");
        IQueryable<User> users = userRepository.GetMany();
        if (word is not null)
        {
            users = users.Where(u => u.Username.ToLower().Contains(word.ToLower()));
        }

        List<User> result = users.ToList();
        if (result.Count == 0)
        {
            Console.WriteLine("No users found.");
            return Task.CompletedTask;
        }
        foreach (User user in result)
        {
            Console.WriteLine($"[{user.Id}] {user.Username}");
        }
        return Task.CompletedTask;
    }
}
