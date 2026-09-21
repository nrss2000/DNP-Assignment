using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class DeleteCommentView
{
    private readonly ICommentRepository commentRepository;

    public DeleteCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        int id = ConsoleInput.ReadInt("Comment id");
        await commentRepository.DeleteAsync(id);
        Console.WriteLine("Comment deleted.");
    }
}
