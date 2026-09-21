using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class UpdateCommentView
{
    private readonly ICommentRepository commentRepository;

    public UpdateCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        int id = ConsoleInput.ReadInt("Comment id");
        Comment existing = await commentRepository.GetSingleAsync(id);
        string? body = ConsoleInput.ReadOptional("New comment text (empty keeps current)");

        await commentRepository.UpdateAsync(new Comment
        {
            Id = existing.Id,
            Body = body ?? existing.Body,
            UserId = existing.UserId,
            PostId = existing.PostId
        });
        Console.WriteLine("Comment updated.");
    }
}
