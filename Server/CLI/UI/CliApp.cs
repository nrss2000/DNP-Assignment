using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly ManageUsersView usersView;
    private readonly ManagePostsView postsView;
    private readonly ManageCommentsView commentsView;

    public CliApp(IUserRepository userRepository, IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        usersView = new ManageUsersView(userRepository);
        postsView = new ManagePostsView(postRepository, userRepository, commentRepository);
        commentsView = new ManageCommentsView(commentRepository, postRepository, userRepository);
    }

    public async Task StartAsync()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("=== Main menu ===");
            Console.WriteLine("1) Manage users");
            Console.WriteLine("2) Manage posts");
            Console.WriteLine("3) Manage comments");
            Console.WriteLine("0) Exit");

            switch (ConsoleInput.ReadInt("Choose"))
            {
                case 1: await usersView.ShowAsync(); break;
                case 2: await postsView.ShowAsync(); break;
                case 3: await commentsView.ShowAsync(); break;
                case 0: running = false; break;
                default: Console.WriteLine("Unknown option."); break;
            }
        }
        Console.WriteLine("Bye!");
    }
}
