using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tweet.Models;
using Tweet.Repository;

namespace Tweet.Controllers;

[Authorize]
public class FollowController : Controller
{
    private readonly IFollowRepository _followRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public FollowController(IFollowRepository followRepository, UserManager<ApplicationUser> userManager)
    {
        _followRepository = followRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if(user == null)
            return NotFound();

        var followers = await _followRepository.GetFollowersAsync(user.Id);
        var following = await _followRepository.GetFollowingAsync(user.Id);

        var model = new FollowViewModel
        {
            User = user,
            Followers = followers.ToList(),
            Following = following.ToList()
        };
        
        return View(model);
    }

    public async Task<IActionResult> Follow(int followeeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || user.Id == followeeId)
            return BadRequest();
        
        var isFollowing = await _followRepository.IsFollowingAsync(user.Id, followeeId);

        if (!isFollowing)
        {
            var follow = new Follow
            {
                FollowerId = user.Id,
                FolloweeId = followeeId
            };
            
            await _followRepository.AddAsync(follow);
        }
        
        return RedirectToAction("Index", "User", new { UserId = followeeId });   
    }

    public async Task<IActionResult> Unfollow(int followeeId)
    {
        var user = await _userManager.GetUserAsync(User);
        if(user == null || user.Id == followeeId)
            return BadRequest();

        var follow = new Follow
        {
            FollowerId = user.Id,
            FolloweeId = followeeId
        };
        
        await _followRepository.RemoveAsync(follow);
        
        return RedirectToAction("Index", "User", new { userId = followeeId });
    }
}