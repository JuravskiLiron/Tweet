namespace Tweet.Models;

public class Notification
{
     public int Id { get; set; }
     
     public ApplicationUser User { get; set; }
     public int RecipientId { get; set; }
     
     public ApplicationUser Sender { get; set; }
     public int? SenderId { get; set; }
     
   //  public string Message { get; set; }
     
     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
     
     public bool IsRead { get; set; } = false;
}