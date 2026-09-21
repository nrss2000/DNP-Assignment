using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly CreatePostView createPostView;
    private readonly ListPostsView listPostsView;
    private readonly SinglePostView singlePostView;
    private readonly UpdatePostView updatePostView;
    private readonly DeletePostView deletePostView;

    public ManagePostsView(IPostRepository postRepository, IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        createPostView = new CreatePostView(postRepository, userRepository);
        listPostsView = new ListPostsView(postRepository);
        singlePostView = new SinglePostView(postRepository, commentRepository);
        updatePostView = new UpdatePostView(postRepository);
        deletePostView = new DeletePostView(postRepository, commentRepository);
    }

    public async Task ShowAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.WriteLine();
            Console.WriteLine("=== Manage posts ===");
            Console.WriteLine("1) Create post");
            Console.WriteLine("2) Posts overview (optional user id filter)");
            Console.WriteLine("3) View single post");
            Console.WriteLine("4) Update post");
            Console.WriteLine("5) Delete post");
            Console.WriteLine("0) Back");

            try
            {
                switch (ConsoleInput.ReadInt("Choose"))
                {
                    case 1: await createPostView.ShowAsync(); break;
                    case 2: await listPostsView.ShowAsync(); break;
                    case 3: await singlePostView.ShowAsync(); break;
                    case 4: await updatePostView.ShowAsync(); break;
                    case 5: await deletePostView.ShowAsync(); break;
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
