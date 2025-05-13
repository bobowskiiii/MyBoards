using System.ComponentModel.DataAnnotations.Schema;

namespace MyBoardss.Entities;

public class WorkState
{
    public Guid Id { get; set; }
    public string State { get; set; }
    public virtual List<WorkItem> WorkItems { get; set; }
}