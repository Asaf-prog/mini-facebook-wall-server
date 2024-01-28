namespace facebook_wall_project.Models;

public class Post
{
    public int PostId { get; set; }
    public string Header { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set;}

    public int UserId { get; set; } 

     public ICollection<Like> Likes { get; set; }

      public User User { get; set; }

}