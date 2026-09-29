using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class DeletePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public DeletePostView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        int id = ConsoleInput.ReadInt("Post id");
        await postRepository.DeleteAsync(id);

        foreach (Comment comment in commentRepository.GetMany().Where(c => c.PostId == id).ToList())
        {
            await commentRepository.DeleteAsync(comment.Id);
        }
        Console.WriteLine("Post (and its comments) deleted.");
    }
}
