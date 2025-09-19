using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Tweet.Models;
using Tweet.Repository;

namespace Tweet.Controllers;

public class LikeController : Controller
{
    private readonly ILikeRepository _likeRepository;
    private readonly IPostRepository _postRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public LikeController(ILikeRepository likeRepository, IPostRepository postRepository,
        UserManager<ApplicationUser> userManager)
    {
        _likeRepository = likeRepository;
        _postRepository = postRepository;
        _userManager = userManager;
            
        
    }

    [HttpPost]
    public async Task<IActionResult> LikePost(int PostId)
    {
        var user = await _userManager.GetUserAsync(User);
        if(user == null)
            return Unauthorized();
        
        var post = await _postRepository.GetByIdAsync(PostId);
        if(post == null)
            return NotFound();

        var isLiked = await _likeRepository.IsLikedAsync(user.Id, PostId);
        if (!isLiked)
        {
            var like = new Like
            {
                UserId = user.Id,
                PostId = PostId,
            };

            await _likeRepository.AddLikeAsync(like);
        }

        return RedirectToAction("Index", "Post");
    }

    [HttpPost]
    public async Task<IActionResult> UnlikePost(int PostId)
    {
        var user = await _userManager.GetUserAsync(User);
        if(user == null)
            return Unauthorized();
        
        var post = await _postRepository.GetByIdAsync(PostId);
        if(post == null)
            return NotFound();
        
        var like = await _likeRepository.GetLikeAsync(user.Id, PostId);
        if(like != null)
            await _likeRepository.RemoveLikeAsync(like);
        
        return RedirectToAction("Index", "Post");
    }
}


























