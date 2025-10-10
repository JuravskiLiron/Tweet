using Tweet.Models;

namespace Tweet.Repository;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetByPostIdAsync(int postId);
    Task<IEnumerable<Comment>> GetCommentsForPostAsync(int postId, int? currentUserId = null);
    Task<Comment?> GetByIdAsync(int id);
    Task AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(int id);
    
    Task<bool> ToggleLikeAsync(int commentId, int userId);
}