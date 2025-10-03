using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Tweet.Models;
using Tweet.Repository;

namespace Tweet.Controllers;
[Authorize]
public class PostController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<PostController> _logger;
    private readonly ILikeRepository _likeRepository;
    public readonly ICommentRepository _commentRepository;

    public PostController(IPostRepository postRepository, UserManager<ApplicationUser> userManager, ILogger<PostController> logger, ILikeRepository likeRepository)
    {
        _postRepository = postRepository;
        _userManager = userManager;
        _logger = logger;
        _likeRepository = likeRepository;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _postRepository.GetPostsAsync();
        var user = await _userManager.GetUserAsync(User);
        var result = new List<PostViewModel>();

        foreach (var post in posts)
        {
            var isLiked = await _likeRepository.IsLikedAsync(user.Id, post.Id);
            var likeCount = await _likeRepository.GetLikeCountAsync(post.Id);
            result.Add(new PostViewModel
            {
                Post = post,
                IsLikedByCurrentUser = isLiked,
                LikeCount = likeCount
            });
        }
        
        return View(result);
    }
    
    

    public IActionResult Create()
    {
        
        
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Post post)
    {
       // if (ModelState.IsValid)
        //{
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
                return RedirectToAction("Login", "Account");
            post.UserId = user.Id;
            post.CreatedAt = DateTime.UtcNow;
            
            await _postRepository.AddAsync(post);
            return RedirectToAction("Index");
        //}
        
        return View(post);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        
        if(post == null)
            return Forbid();
        
        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Post post)
    {
        if (id != post.Id)
            return NotFound();
        
        var user = await _userManager.GetUserAsync(User);
        _logger.LogInformation("post.UserId = {postUserId}, user.Id = {User.Id}", post.UserId, user.Id);
        if (post.UserId != user.Id)
        {
            _logger.LogWarning("post.UserId = {postUserId}, user.Id = {User.Id}", post.UserId, post.Id);
            return Forbid();
        }

        //if  (ModelState.IsValid)
       // {
            await _postRepository.UpdateAsync(post);
            return RedirectToAction("Index");
        //}
        
       // return View(post);
        
            
        
    }

    public async Task<IActionResult> Delete(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        var user = await _userManager.GetUserAsync(User);
        if (post == null || post.UserId != user.Id)
            return Forbid();
        
        return View(post);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);

        if (post.UserId != int.Parse(_userManager.GetUserId(User)))
            return Forbid();
        
        await _postRepository.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if(post == null)
            return NotFound();
        
        var user = await _userManager.GetUserAsync(User);
        var isLiked = await _likeRepository.IsLikedAsync(user.Id, post.Id);
        var LikeCount = await _likeRepository.GetLikeCountAsync(post.Id);
        var comments = await _commentRepository.GetByPostIdAsync(post.Id);
        var model = new PostDetailsView
        {
            Post = post,
            isLikedByCurrentUser = isLiked,
            LikeCount = LikeCount,
            Comments = comments
        };
        return View(model);
    }
    
}