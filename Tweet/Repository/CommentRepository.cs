using Microsoft.EntityFrameworkCore;
using Tweet.Data;
using Tweet.Models;

namespace Tweet.Repository;

public class CommentRepository
{
    private readonly ApplicationDBContext _context;

    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.FindAsync(id);
        
    }

    public async Task<IEnumerable<Comment>> GetByPostIdAsync(int postId)
    {
        return await _context.Comments
            .Where(c => c.PostId == postId && c.ParentCommentId == null)
            .Include(c => c.User)
            .Include(c => c.Likes)
            .Include(c => c.Replies).ThenInclude(r => r.User)
            .Include(c => c.Replies).ThenInclude(r => r.Likes)
            .ToListAsync();
    }

    public async Task<bool> ToggleLikeAsync(int commentId, int userId)
    {
        var existingLike = await _context.CommentLikes
                
            .FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);
        if (existingLike != null)
        {
            _context.CommentLikes.Remove(existingLike);
            await _context.SaveChangesAsync();
            return true;
        }

        var like = new CommentLike
        {
            CommentId = commentId,
            UserId = userId,
        };

        var comment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment != null && comment.UserId == userId) 
        {
            //var sender = await _context.Users.FindAsync(userId);
            var notification = new Notification
            {
                RecipientId = comment.UserId,
                SenderId = userId,
               // Message = $"{sender.UserName} liked your comment:\"(comment id)\"",
                CreatedAt = DateTime.Now,
                IsRead  = false,
            };
            
            _context.Notifications.Add(notification);
        }
        _context.CommentLikes.Add(like);
        await _context.SaveChangesAsync();
        return true;
     
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId, int? currentUserId = null)
    {
        var comments = await _context.Comments
            .Where(c => c.PostId == postId)
            .Include(c => c.User)
            .Include(c => c.Likes)
            .ToListAsync();

        foreach (var comment in comments)
        {
            comment.IsLikedByCurrentUser = comment.Likes.Any(I => I.UserId == currentUserId);
        }
        
        return comments;
    }
}