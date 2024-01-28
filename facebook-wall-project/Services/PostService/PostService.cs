using facebook_wall_project.Data;
using facebook_wall_project.Models;
using Microsoft.EntityFrameworkCore;

public class PostService : IPostService
{
    private readonly AppDbContext _dbContext;

    public PostService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreatePost(CreatePostViewModel model)
    {
        var newPost = new Post
                {
                    Header = model.Header,
                    Description = model.Description,
                    UserId = model.UserId,
                    CreatedAt = DateTime.Now,
                    Likes = new List<Like>()
                };

        _dbContext.Posts.Add(newPost);

        _dbContext.SaveChanges();
    }

    public bool ValidateId(int postId)
    {
        var post = _dbContext.Posts.Find(postId);
        return post == null;
    }

    public void CreateNewLike(int postId, int userId)
    {
        var newLike = new Like
        {
            PostId = postId,
            UserId= userId,
            LikedAt = DateTime.Now
        };
        _dbContext.Likes.Add(newLike);
        _dbContext.SaveChanges();
    }

     public IEnumerable<PostDto> GetUserPosts(int userId)
     {
        var userPosts = _dbContext.Posts
        .Include(p => p.Likes)
        .Where(p => p.UserId == userId)
        .OrderByDescending(p => p.CreatedAt) 
        .ToList();

        var postDtos = userPosts
        .Where(post => post != null) 
        .Select(post => new PostDto.Builder()
            .WithPostId(post.PostId)
            .WithUserId(post.UserId)
            .WithHeader(post.Header)
            .WithDescription(post.Description)
            .WithLikeCount(post.Likes.Count)
            .WithPostTime(post.CreatedAt)
            .Build()
        )
        .ToList();

        return postDtos;
     }

     public int GetLikesCount(int postId)
     {
        if (_dbContext.Likes != null)
        {
            int likesCount = _dbContext.Likes.Count(l => l.PostId == postId);
            return likesCount;
        }
        return 0;
    }

    public IEnumerable<PostDto> GetAllPost()
    {
            var Posts = _dbContext.Posts
            .Include(p => p.Likes)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        var postDtos = Posts
            .Where(post => post != null)
            .Select(post => new PostDto.Builder()
                .WithPostId(post.PostId)
                .WithUserId(post.UserId)
                .WithHeader(post.Header)
                .WithDescription(post.Description)
                .WithLikeCount(post.Likes.Count)  
                .WithPostTime(post.CreatedAt)
                .Build()
            )
            .ToList();

        return postDtos;
    }

    public IEnumerable<LikeDto> GetLikeByPostId(int postId)
    {
         var likeCounts = _dbContext.Likes
        .Where(like => like.PostId == postId)
        .GroupBy(like => like.LikedAt.Date)
        .Select(group => new LikeDto.Builder()
            .WithLikedAt(group.Key)
            .WithLikeCount(group.Count())
            .Build()
        )
        .ToList();

    return likeCounts;
    }
}
