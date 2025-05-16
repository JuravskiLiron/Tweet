namespace Tweet.Models;

public class Follow
{
    public int Id { get; set; }
    
    public int FollowerId { get; set; }
    public ApplicationUser Follower { get; set; }
    
    public int FolloweeId { get; set; }
    public ApplicationUser Followee { get; set; }
}