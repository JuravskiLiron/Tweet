using Microsoft.EntityFrameworkCore;
using Tweet.Data;
using Tweet.Models;

namespace Tweet.Repository;

public class LikeRepository : ILikeRepository
{
    public readonly ApplicationDBContext _context;

    public LikeRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task AddLikeAsync(Like like)
    {
        _context.Likes.Add(like);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveLikeAsync(Like like)
    {
        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
    }

    public async Task<Like> GetLikeAsync(int UserId, int PostId)
    {
        return await (_context.Likes.FirstOrDefaultAsync(I => I.UserId == UserId && I.PostId == PostId));
    }

    public async Task<int> GetLikeCountAsync(int PostId)
    {
        return await (_context.Likes.CountAsync(I => I.PostId == PostId)); 
    }

    public async Task<bool> IsLikedAsync(int userId, int PostId)
    {
        return await (_context.Likes.AnyAsync(I => I.UserId == userId && I.PostId == PostId));
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}