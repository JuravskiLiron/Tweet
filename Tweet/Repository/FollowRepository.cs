using Microsoft.EntityFrameworkCore;
using Tweet.Data;
using Tweet.Models;

namespace Tweet.Repository;

public class FollowRepository : IFollowRepository
{
    private readonly ApplicationDBContext _context;

    public FollowRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<bool> IsFollowingAsync(int followerId, int followeeId)
    {
        return await _context.Follows.AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
    }

    public async Task<IEnumerable<ApplicationUser>> GetFollowersAsync(int userId)
    {
        return await _context.Follows
            .Where(f => f.FolloweeId == userId)
            .Select(f => f.Follower)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<ApplicationUser>> GetFollowingAsync(int userId)
    {
        return await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.Followee)
            .ToListAsync();
    }

    public async Task AddAsync(Follow follow)
    {
        _context.Follows.Add(follow);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Follow follow)
    {
        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();
    }
}



















