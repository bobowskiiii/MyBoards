namespace MyBoardss.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string Author { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
    public WorkItem WorkItem { get; set; }
    
    // EF i tak utworzyłby tą relację
    public int WorkItemId { get; set; }
    
}