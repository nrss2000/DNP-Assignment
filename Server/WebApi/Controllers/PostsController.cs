using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public PostsController(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto dto)
    {
        try
        {
            User author = await userRepository.GetSingleAsync(dto.UserId);

            Post post = new Post
            {
                Title = dto.Title,
                Body = dto.Body,
                UserId = dto.UserId
            };
            Post added = await postRepository.AddAsync(post);

            PostDto postDto = new PostDto
            {
                Id = added.Id,
                Title = added.Title,
                Body = added.Body,
                UserId = added.UserId,
                AuthorUsername = author.Username
            };
            return Created($"/posts/{postDto.Id}", postDto);
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
    public ActionResult<IEnumerable<PostDto>> GetPosts([FromQuery] string? title, [FromQuery] int? userId,
        [FromQuery] string? username)
    {
        try
        {
            IQueryable<Post> posts = postRepository.GetMany();

            if (!string.IsNullOrWhiteSpace(title))
            {
                posts = posts.Where(p => p.Title.Contains(title));
            }

            if (userId != null)
            {
                posts = posts.Where(p => p.UserId == userId);
            }

            if (!string.IsNullOrWhiteSpace(username))
            {
                List<int> matchingUserIds = userRepository.GetMany()
                    .Where(u => u.Username.Contains(username))
                    .Select(u => u.Id)
                    .ToList();
                posts = posts.Where(p => matchingUserIds.Contains(p.UserId));
            }

            List<PostDto> postDtos = new List<PostDto>();
            foreach (Post post in posts)
            {
                User author = userRepository.GetSingleAsync(post.UserId).Result;
                postDtos.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    UserId = post.UserId,
                    AuthorUsername = author.Username
                });
            }

            return Ok(postDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetSinglePost(int id)
    {
        try
        {
            Post post = await postRepository.GetSingleAsync(id);
            User author = await userRepository.GetSingleAsync(post.UserId);

            PostDto postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                AuthorUsername = author.Username
            };
            return Ok(postDto);
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
    public async Task<ActionResult> UpdatePost(int id, UpdatePostDto dto)
    {
        try
        {
            Post existing = await postRepository.GetSingleAsync(id);
            existing.Title = dto.Title;
            existing.Body = dto.Body;
            await postRepository.UpdateAsync(existing);
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
    public async Task<ActionResult> DeletePost(int id)
    {
        try
        {
            await postRepository.DeleteAsync(id);
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
