using facebook_wall_project.Data;
using facebook_wall_project.Models;
using Microsoft.EntityFrameworkCore;


public class UserService : IUserService
{
    private readonly AppDbContext _appDbContext;

    public UserService(AppDbContext dbContext)
    {
        _appDbContext = dbContext;
    }

    public void CreateUser(UserViewModel userModel)
    {
        var user = new User
        {
            UserName = userModel.UserName,
            IsAdmin = userModel.IsAdmin,
            Posts = new List<Post>()
        };

       // I am aware of the possibility that two users might try to insert with the same username. 
       // While it may succeed without synchronization, in cases of multiple server processes, even with synchronization, it may not help.
       // That's why I prefer to set the userName as a primary key.
         
         _appDbContext.Users.Add(user);

        _appDbContext.SaveChanges();
    }

    public bool UserExist(string name)
    {
        if(_appDbContext.Users != null)
        {
            return _appDbContext.Users.Any(u => u.UserName == name);
        }
        return true;
    }

     public User GetCurrentUser(int userId)
     {
        var user = _appDbContext.Users.Find(userId);
        return user;
     }

     public bool ValidateId(int userId)
     {
         var user = _appDbContext.Users.Find(userId);

         return user == null;
     }

     public bool AlreadyLiked(int postId, int userId)
     {
         Post post = _appDbContext.Posts.Find(postId);
         if(post == null)
         {
            return false;
         }

         bool userLikedPost = _appDbContext.Likes
            .Any(like => like.UserId == userId && postId == like.PostId);
         
         return post.UserId == userId || userLikedPost;
    }

    public int GetUserIdByName(string name)
    {
        if (_appDbContext.Users != null)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.UserName == name);

            return user?.UserId ?? -1;
        }

        return -1;
    }
     public bool IsUserAdmin(int userId)
    {
        var user = _appDbContext.Users.FirstOrDefault(u => u.UserId == userId);

        if (user != null)
        {
            return user.IsAdmin;
        }

        return false;
    }

}