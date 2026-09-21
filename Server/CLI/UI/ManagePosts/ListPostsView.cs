using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public Task ShowAsync()
    {
        int? userId = ConsoleInput.ReadOptionalInt("Only posts by user id (leave empty for all)");
        IQueryable<Post> posts = postRepository.GetMany();
        if (userId is not null)
        {
            posts = posts.Where(p => p.UserId == userId);
        }

        List<Post> result = posts.ToList();
        if (result.Count == 0)
        {
            Console.WriteLine("No posts found.");
            return Task.CompletedTask;
        }
        foreach (Post post in result)
        {
            Console.WriteLine($"[{post.Title}, {post.Id}]");
        }
        return Task.CompletedTask;
    }
}
