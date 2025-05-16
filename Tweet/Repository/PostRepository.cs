using Microsoft.EntityFrameworkCore;
using Tweet.Data;
using Tweet.Models;

namespace Tweet.Repository;

public class PostRepository : IPostRepository
{
    private readonly ApplicationDBContext _context;

    public PostRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        return await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        return await _context.Posts.Include(p => p.User).ToListAsync();
    }

    public async Task AddAsync(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}












