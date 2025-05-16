using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

    public PostController(IPostRepository postRepository, UserManager<ApplicationUser> userManager, ILogger<PostController> logger)
    {
        _postRepository = postRepository;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _postRepository.GetPostsAsync();
        
        
        
        
        
        return View(posts);
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

        if (post.UserId != user.Id)
            return Forbid();

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
    
}