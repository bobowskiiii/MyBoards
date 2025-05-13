namespace MyBoardss.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Value { get; set; }
    
    public virtual List<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
    // public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
}