namespace Tweet.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    //Внешний ключ "User"
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public string? ImagePath { get; set; }
}