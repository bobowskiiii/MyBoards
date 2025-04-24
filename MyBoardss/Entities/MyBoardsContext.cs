using Microsoft.EntityFrameworkCore;

namespace MyBoardss.Entities;

public class MyBoardsContext : DbContext
{
    public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
    {
    }

    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<Epic> Epics { get; set; }
    public DbSet<Task> Tasks { get; set; }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<WorkState> WorkStates { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkState>()
            .Property(s => s.State)
            .IsRequired()
            .HasMaxLength(60);  
        
        modelBuilder.Entity<Epic>().Property(x => x.EndDate).HasPrecision(3);
        modelBuilder.Entity<Task>().Property(x => x.Activity).HasMaxLength(200);
        modelBuilder.Entity<Task>().Property(x => x.RemainingWork).HasPrecision(14, 2);
        modelBuilder.Entity<Issue>().Property(x => x.Effort).HasColumnType("decimal(5,2)");
        
        modelBuilder.Entity<WorkItem>(eb =>
        {
            eb.HasOne(w => w.State)
                .WithMany()
                .HasForeignKey(w => w.StateId);
            
            eb.Property(x => x.Area).HasColumnName("varchar(200)");
            eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
            eb.Property(x => x.Priority).HasDefaultValue(1);
            eb.HasMany(w => w.Comments)
                .WithOne(c => c.WorkItem)
                .HasForeignKey(c => c.WorkItemId);

            eb.HasOne(w => w.User)
                .WithMany(u => u.WorkItems)
                .HasForeignKey(w => w.UserId);

            eb.HasMany(w => w.Tags)
                .WithMany(t => t.WorkItems)
                .UsingEntity<WorkItemTag>(
                    w => w.HasOne(wit => wit.Tag)
                        .WithMany()
                        .HasForeignKey(wit => wit.TagId),
                    
                    w => w.HasOne(wit => wit.WorkItem)  
                        .WithMany()
                        .HasForeignKey(wit => wit.WorkItemId),
                    wit =>
                    {
                        wit.HasKey(x => new
                        {
                            x.TagId,
                            x.WorkItemId
                        });
                        wit.Property(x => x.PublicationDate).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
                    }
                );
            
        });
    
        modelBuilder.Entity<Comment>(eb =>
        {
            eb.Property(x => x.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();

        });
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId);
        
    }
}