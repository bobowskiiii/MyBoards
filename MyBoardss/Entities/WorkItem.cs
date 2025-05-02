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
    public decimal Efford { get; set; }
}

public class Task : WorkItem
{
    public string Activity { get; set; }
    public decimal RemaningWork { get; set; }
}

public abstract class WorkItem
{
    public int Id { get; set; }
    
    public string Area { get; set; }
    public string IterationPath { get; set; }
    public int Priority { get; set; }
    
    //Epic
    
    //Issue
    
    
    //Task
  

    public string Type { get; set; }
    
    public List<Comment> Comments { get; set; } = new List <Comment>();
    
    public User User { get; set; }
    public Guid AuthorId { get; set; }
    
    public List<Tag> Tags { get; set; } = new List<Tag>();
    public WorkState State { get; set; }
    public Guid StateId { get; set; }
}