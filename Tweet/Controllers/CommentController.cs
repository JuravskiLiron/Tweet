using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tweet.Data;
using Tweet.Models;
using Tweet.Repository;

namespace Tweet.Controllers;

public class CommentController : Controller
{
    private readonly ICommentRepository _commentRepository;
    private readonly UserManager<ApplicationUser> _UserManager;
    private readonly IPostRepository _postRepository;
    private readonly ApplicationDBContext _context;

    public CommentController(ICommentRepository commentRepository, UserManager<ApplicationUser> userManager,
        IPostRepository postRepository, ApplicationDBContext context)
    {
        _commentRepository = commentRepository;
        _UserManager = userManager;
        _postRepository = postRepository;
        _context = context;
    }

    public async Task<IActionResult> GetComments(int postId)
    {
        var post = _commentRepository.GetByPostIdAsync(postId);
        if(post == null)
            return NotFound();
        var comments = await _commentRepository.GetByPostIdAsync(postId);
        return PartialView("_CommentsPartial", comments);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(int postId, string content, int? parentComentId)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            ModelState.AddModelError("content", "Content is required");
            return BadRequest(ModelState);
        }
        
        var user = await _UserManager.GetUserAsync(User);
        var Comment = new Comment
        {
            PostId = postId,
            Content = content,
            CreatedDate = DateTime.UtcNow,
            UserId = user.Id,
            ParentCommentId = parentComentId
        };
            await _commentRepository.AddAsync(Comment);
            return RedirectToAction("Details", "Post", new { id = postId });
    }
    
    [HttpGet]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        var user = await _UserManager.GetUserAsync(User);
        if(comment == null)
            return NotFound();
        if(comment == null || user.Id != comment.UserId)
            return Forbid();
        
        return View(comment);
    }
    
    [HttpPost]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId);
        if(comment == null)
            return NotFound();
        
        var user = await _UserManager.GetUserAsync(User);
        if(user.Id != comment.UserId)
            return Forbid();
    }
    
    
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
}