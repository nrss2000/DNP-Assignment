using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class UpdatePostView
{
    private readonly IPostRepository postRepository;

    public UpdatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task ShowAsync()
    {
        int id = ConsoleInput.ReadInt("Post id");
        Post existing = await postRepository.GetSingleAsync(id);

        string? title = ConsoleInput.ReadOptional($"New title (empty keeps '{existing.Title}')");
        string? body = ConsoleInput.ReadOptional("New body (empty keeps current)");

        await postRepository.UpdateAsync(new Post
        {
            Id = existing.Id,
            Title = title ?? existing.Title,
            Body = body ?? existing.Body,
            UserId = existing.UserId
        });
        Console.WriteLine("Post updated.");
    }
}
