using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ManageCommentsView
{
    private readonly AddCommentView addCommentView;
    private readonly ListCommentsView listCommentsView;
    private readonly UpdateCommentView updateCommentView;
    private readonly DeleteCommentView deleteCommentView;

    public ManageCommentsView(ICommentRepository commentRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        addCommentView = new AddCommentView(commentRepository, postRepository, userRepository);
        listCommentsView = new ListCommentsView(commentRepository);
        updateCommentView = new UpdateCommentView(commentRepository);
        deleteCommentView = new DeleteCommentView(commentRepository);
    }

    public async Task ShowAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine();
            Console.WriteLine("=== Manage comments ===");
            Console.WriteLine("1) Add comment to post");
            Console.WriteLine("2) List comments (optional user id / post id filter)");
            Console.WriteLine("3) Update comment");
            Console.WriteLine("4) Delete comment");
            Console.WriteLine("0) Back");

            try
            {
                switch (ConsoleInput.ReadInt("Choose"))
                {
                    case 1: await addCommentView.ShowAsync(); break;
                    case 2: await listCommentsView.ShowAsync(); break;
                    case 3: await updateCommentView.ShowAsync(); break;
                    case 4: await deleteCommentView.ShowAsync(); break;
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
