namespace MyBoardss.Entities;

public class WorkState
{
    public Guid Id { get; set; }
    public string State { get; set; }
    public List<WorkItem> WorkItems { get; set; }
}