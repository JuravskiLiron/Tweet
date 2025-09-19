namespace Tweet.Models;

public class PostDetailsView
{
    public Post Post { get; set; }
    public bool isLikedByCurrentUser { get; set; }
    public int LikeCount { get; set; }

    public IEnumerable<Comment> Comments { get; set; }
}