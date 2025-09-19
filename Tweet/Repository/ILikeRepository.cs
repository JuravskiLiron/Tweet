using AutoMapper.Internal.Mappers;
using Tweet.Models;

namespace Tweet.Repository
{

    public interface ILikeRepository
    {
        Task<bool> IsLikedAsync(int userId, int PostId);
        Task<int> GetLikeCountAsync(int PostId);
        Task AddLikeAsync(Like like);
        Task RemoveLikeAsync(Like like);
        Task<Like> GetLikeAsync(int UserId, int PostId);
    }
}