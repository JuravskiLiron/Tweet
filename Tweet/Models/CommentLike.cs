namespace Tweet.Models;

public class CommentLike
{
    public int Id { get; set; }

    public ApplicationUser User { get; set; }
    public int UserId { get; set; }

    public int CommentId { get; set; }
    public Comment Comment { get; set; }
}