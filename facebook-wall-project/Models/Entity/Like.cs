namespace facebook_wall_project.Models;

public class Like
{
    public int LikeId { get; set; }

    public int PostId { get; set; }
    public int UserId { get; set; }

    public Post Post { get; set; }
    public DateTime LikedAt  { get; set;}

}