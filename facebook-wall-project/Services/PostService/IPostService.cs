using facebook_wall_project.Models;

public interface IPostService
{
    public void CreatePost(CreatePostViewModel model);
    public bool ValidateId(int postId);
    public void CreateNewLike(int postId, int userId);
    public IEnumerable<PostDto> GetUserPosts(int userId);
    public IEnumerable<PostDto> GetAllPost();
    public int GetLikesCount(int postId);
    public IEnumerable<LikeDto> GetLikeByPostId(int postId);
}