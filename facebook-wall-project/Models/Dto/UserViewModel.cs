using System.ComponentModel.DataAnnotations;
namespace facebook_wall_project.Models;

public class UserViewModel
{
    public UserViewModel(){}
     public int UserId { get; set; }

    [Required(ErrorMessage = "Please enter first and last name.")]
   [StringLength(50, ErrorMessage = "First name should not exceed 50 characters.")]
    public string UserName { get; set; }

    public bool IsAdmin{ get; set; }

}