using Tweet.Models;

namespace Tweet.Repository;

public interface IPostRepository
{
    Task<Post> GetByIdAsync(int id);
    Task<IEnumerable<Post>> GetPostsAsync();
    Task AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(int id);
}