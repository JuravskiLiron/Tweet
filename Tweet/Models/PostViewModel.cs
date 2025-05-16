namespace Tweet.Models;

public class PostViewModel
{
    public Post Post { get; set; }
    public int LikeCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
}