using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using facebook_wall_project.Models;
using Microsoft.AspNetCore.SignalR;

namespace facebook_wall_project.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IPostService _postService;
     private readonly IHubContext<LikeHub> _hubContext;
    
    public HomeController(
        ILogger<HomeController> logger,
         IUserService userService,
          IPostService postService,
          IHubContext<LikeHub> hubContext)
    {
        _logger = logger;
        _userService = userService;
        _postService = postService;
        _hubContext = hubContext;
    }

    [HttpPost]
    [Route("Create-New-User")]
    public IActionResult CreateNewUser([FromBody] UserViewModel userModel)
    {
        try
        {
            _logger.LogInformation($"UserName: {userModel.UserName}");

            if (ModelState.IsValid)
            {
                if (_userService.UserExist(userModel.UserName))
                {
                    ModelState.AddModelError("UserName", "Username already exists");
                    return BadRequest(ModelState);
                }
                _userService.CreateUser(userModel);

                 return Ok(new { Message = "User created successfully" });
            }

             return BadRequest(new { Errors = ModelState });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating user: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    [Route("Create_New_Post")]
    public IActionResult CreateAPost([FromBody] CreatePostViewModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                model.UserId = _userService.GetUserIdByName(model.userName);
                User currentUser =  _userService.GetCurrentUser(model.UserId); 

                if (currentUser != null)
                { 
                    _postService.CreatePost(model);

                    return Ok(new { Message = "Post created successfully" });
                }
                else
                {
                    return Unauthorized(new {Message = "User not authenticated"});
                }
                    
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new post.");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("user/{userId}/posts")]
    public IActionResult GetUserPosts(string userId)
    {
        var user = _userService.GetCurrentUser(int.Parse(userId));

        if (user == null)
        {
            return NotFound($"User with ID {userId} not found.");
        }
        var userPosts = _postService.GetUserPosts(int.Parse(userId));
    
        return Ok(userPosts);
    }

    [HttpGet("admin/likes-per-day/{postId}")]
    public IActionResult GetLikesByPostId(int postId)
    {
        try
        {
            var likes = _postService.GetLikeByPostId(postId);
            return Ok(likes);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred: {ex.Message}", ex);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("admin/all_posts")]
    public IActionResult GetAllPosts()
    {
       try
        {
            var posts = _postService.GetAllPost();
            _logger.LogInformation("All posts retrieved successfully.");
            return Ok(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while retrieving all posts: {ex.Message}", ex);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("userLogin/{userName}")]
    public IActionResult CheckIfUserExists(string userName)
    {
        try
        {
            int userId = _userService.GetUserIdByName(userName);
            if(userId == -1)
            {
                return Ok(new { isExist = false });
            }
            bool isAdmin = _userService.IsUserAdmin(userId);
          
           return Ok(new { isExist = true, isAdmin = isAdmin });
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred: {ex.Message}", ex);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("user_name/{userName}/posts")]
    public IActionResult GetUserPostsByName(string userName)
    {
        try
        {
            int userId = _userService.GetUserIdByName(userName);
           
            if(userId == -1)
            {
                  throw new InvalidOperationException($"User with username '{userName}' not found");
            }

            var userPosts = _postService.GetUserPosts(userId);

            return Ok(userPosts);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error retrieving user's posts: {ex.Message}");
            return NotFound($"User with username '{userName}' not found");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving user's posts: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    [Route("Like_Post")]
    public IActionResult LikePost(int postId, string userName)
    {
        try
        {
            if (_postService.ValidateId(postId))
            {
                throw new NotFoundException("Post not found");
            }
             int userId = _userService.GetUserIdByName(userName);

            if (_userService.ValidateId(userId))
            {
                throw new NotFoundException("User not found");
            }

            if (_userService.AlreadyLiked(postId, userId))
            {
                throw new InvalidOperationException("User already liked the post");
            }

            _postService.CreateNewLike(postId, userId);
            
            int likesCount = _postService.GetLikesCount(postId);
            _hubContext.Clients.All.SendAsync("ReceiveLikesUpdate", postId, likesCount);

            return Ok(new { Message = "Add new like successfully" });
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Not found exception in LikePost method");
            return NotFound(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation exception in LikePost method");
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected exception occurred in LikePost method");
            return StatusCode(500, new { Message = "Internal Server Error" });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //get a unique identifier for the current request. This identifier is often used for tracking and debugging purposes.
    }
}
