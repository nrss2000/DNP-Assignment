using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository commentRepository;

    public ListCommentsView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public Task ShowAsync()
    {
        int? userId = ConsoleInput.ReadOptionalInt("Only comments by user id (leave empty for all)");
        int? postId = ConsoleInput.ReadOptionalInt("Only comments on post id (leave empty for all)");

        IQueryable<Comment> comments = commentRepository.GetMany();
        if (userId is not null)
        {
            comments = comments.Where(c => c.UserId == userId);
        }
        if (postId is not null)
        {
            comments = comments.Where(c => c.PostId == postId);
        }

        List<Comment> result = comments.ToList();
        if (result.Count == 0)
        {
            Console.WriteLine("No comments found.");
            return Task.CompletedTask;
        }
        foreach (Comment comment in result)
        {
            Console.WriteLine($"[{comment.Id}] post {comment.PostId}, user {comment.UserId}: {comment.Body}");
        }
        return Task.CompletedTask;
    }
}
