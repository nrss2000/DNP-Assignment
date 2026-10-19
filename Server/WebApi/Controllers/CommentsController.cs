using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public CommentsController(ICommentRepository commentRepository, IUserRepository userRepository,
        IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment(CreateCommentDto dto)
    {
        try
        {
            User author = await userRepository.GetSingleAsync(dto.UserId);
            await postRepository.GetSingleAsync(dto.PostId);

            Comment comment = new Comment
            {
                Body = dto.Body,
                UserId = dto.UserId,
                PostId = dto.PostId
            };
            Comment added = await commentRepository.AddAsync(comment);

            CommentDto commentDto = new CommentDto
            {
                Id = added.Id,
                Body = added.Body,
                UserId = added.UserId,
                AuthorUsername = author.Username,
                PostId = added.PostId
            };
            return Created($"/comments/{commentDto.Id}", commentDto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments([FromQuery] int? userId, [FromQuery] string? username,
        [FromQuery] int? postId)
    {
        try
        {
            IQueryable<Comment> comments = commentRepository.GetMany();

            if (userId != null)
            {
                comments = comments.Where(c => c.UserId == userId);
            }

            if (!string.IsNullOrWhiteSpace(username))
            {
                List<int> matchingUserIds = userRepository.GetMany()
                    .Where(u => u.Username.Contains(username))
                    .Select(u => u.Id)
                    .ToList();
                comments = comments.Where(c => matchingUserIds.Contains(c.UserId));
            }

            if (postId != null)
            {
                comments = comments.Where(c => c.PostId == postId);
            }

            List<CommentDto> commentDtos = new List<CommentDto>();
            foreach (Comment comment in comments)
            {
                User author = userRepository.GetSingleAsync(comment.UserId).Result;
                commentDtos.Add(new CommentDto
                {
                    Id = comment.Id,
                    Body = comment.Body,
                    UserId = comment.UserId,
                    AuthorUsername = author.Username,
                    PostId = comment.PostId
                });
            }

            return Ok(commentDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetSingleComment(int id)
    {
        try
        {
            Comment comment = await commentRepository.GetSingleAsync(id);
            User author = await userRepository.GetSingleAsync(comment.UserId);

            CommentDto commentDto = new CommentDto
            {
                Id = comment.Id,
                Body = comment.Body,
                UserId = comment.UserId,
                AuthorUsername = author.Username,
                PostId = comment.PostId
            };
            return Ok(commentDto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateComment(int id, UpdateCommentDto dto)
    {
        try
        {
            Comment existing = await commentRepository.GetSingleAsync(id);
            existing.Body = dto.Body;
            await commentRepository.UpdateAsync(existing);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        try
        {
            await commentRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}
