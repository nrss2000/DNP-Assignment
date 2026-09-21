using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class AddCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public AddCommentView(ICommentRepository commentRepository, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        int postId = ConsoleInput.ReadInt("Post id");
        await postRepository.GetSingleAsync(postId); // throws if the post does not exist
        int userId = ConsoleInput.ReadInt("User id");
        await userRepository.GetSingleAsync(userId); // throws if the user does not exist
        string body = ConsoleInput.ReadRequired("Comment");

        Comment created = await commentRepository.AddAsync(
            new Comment { Body = body, UserId = userId, PostId = postId });
        Console.WriteLine($"Added comment {created.Id} to post {created.PostId}.");
    }
}
