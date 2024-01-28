using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace facebook_wall_project.Models;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public bool IsAdmin {get;set;}
     public ICollection<Post> Posts { get; set; }
}