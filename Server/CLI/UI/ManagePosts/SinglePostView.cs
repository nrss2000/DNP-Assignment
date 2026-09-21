using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        int id = ConsoleInput.ReadInt("Post id");
        Post post = await postRepository.GetSingleAsync(id);

        Console.WriteLine();
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Author (user id): {post.UserId}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine("Comments:");

        List<Comment> comments = commentRepository.GetMany().Where(c => c.PostId == id).ToList();
        if (comments.Count == 0)
        {
            Console.WriteLine("  (none)");
        }
        foreach (Comment comment in comments)
        {
            Console.WriteLine($"  [{comment.Id}] user {comment.UserId}: {comment.Body}");
        }
    }
}
