using System.Collections.Generic;
using facebook_wall_project.Models;
public interface IUserService
{
    public void CreateUser(UserViewModel userModel);
    public User GetCurrentUser(int userId);
    public bool ValidateId(int userId);
    public bool AlreadyLiked(int postId, int userId);
    public bool UserExist(string name);
    public int GetUserIdByName(string name);
    public bool IsUserAdmin(int userId);
}