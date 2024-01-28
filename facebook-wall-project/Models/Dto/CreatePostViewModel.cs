using System.ComponentModel.DataAnnotations;
namespace facebook_wall_project.Models;

public class CreatePostViewModel
{
    [Required]
    public string Header { get; set; }

    [Required]
    public string Description { get; set; }
    
    [Required]
    public string userName {get;set;}

    public int UserId {get;set;}
}