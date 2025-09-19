namespace Tweet.Models;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; }

    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int PostId { get; set; }
    public Post Post { get; set; }

    public ICollection<CommentLike> Likes { get; set; }

    public int? ParentCommentId { get; set; }
    public Comment ParentComment { get; set; }
    public List<Comment> Replies { get; set; } = new();

    public bool IsLikedByCurrentUser { get; set; }
}