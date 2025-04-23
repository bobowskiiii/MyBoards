using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyBoardss.Entities;

public class Epic : WorkItem
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class Issue : WorkItem
{
    public decimal Effort { get; set; }
}

public class Task : WorkItem
{
    public string Activity { get; set; }
    public decimal RemainingWork { get; set; }
}

public abstract class WorkItem
{
    public int Id { get; set; }
    
    public string Area { get; set; }
    public string IterationPath { get; set; }
    public int Priority { get; set; }
    
    //Epic
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    //Issue
    [Column(TypeName = "decimal(5,2)")]
    public decimal Effort { get; set; }
    
    //Task
    public string Activity{ get; set; }
    public decimal RemainingWork { get; set; }

    public string Type { get; set; }
    
    public List<Comment> Comments { get; set; } = new List <Comment>();
    
    public User User { get; set; }
    public Guid UserId { get; set; }
    
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public WorkState State { get; set; }
    public Guid StateId { get; set; }
}