using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly CreateUserView createUserView;
    private readonly ListUsersView listUsersView;
    private readonly UpdateUserView updateUserView;
    private readonly DeleteUserView deleteUserView;

    public ManageUsersView(IUserRepository userRepository)
    {
        createUserView = new CreateUserView(userRepository);
        listUsersView = new ListUsersView(userRepository);
        updateUserView = new UpdateUserView(userRepository);
        deleteUserView = new DeleteUserView(userRepository);
    }

    public async Task ShowAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine();
            Console.WriteLine("=== Manage users ===");
            Console.WriteLine("1) Create user");
            Console.WriteLine("2) List users (optional username filter)");
            Console.WriteLine("3) Update user");
            Console.WriteLine("4) Delete user");
            Console.WriteLine("0) Back");

            try
            {
                switch (ConsoleInput.ReadInt("Choose"))
                {
                    case 1: await createUserView.ShowAsync(); break;
                    case 2: await listUsersView.ShowAsync(); break;
                    case 3: await updateUserView.ShowAsync(); break;
                    case 4: await deleteUserView.ShowAsync(); break;
                    case 0: back = true; break;
                    default: Console.WriteLine("Unknown option."); break;
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
