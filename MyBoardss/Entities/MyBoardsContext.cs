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
    public DbSet<WorkItemTag> WorkItemTags { get; set; }
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
        modelBuilder.Entity<Task>().Property(x => x.RemaningWork).HasPrecision(14, 2);
        modelBuilder.Entity<Issue>().Property(x => x.Efford).HasColumnType("decimal(5,2)");
        
        modelBuilder.Entity<WorkItem>(eb =>
        {
            eb.HasOne(w => w.State)
                .WithMany()
                .HasForeignKey(w => w.StateId);
            
            eb.Property(x => x.Area).HasColumnName("Area");
            eb.Property(x => x.IterationPath).HasColumnName("Iteration_Path");
            eb.Property(x => x.Priority).HasDefaultValue(1);
            eb.HasMany(w => w.Comments)
                .WithOne(c => c.WorkItem)
                .HasForeignKey(c => c.WorkItemId);

            eb.HasOne(w => w.User)
                .WithMany(u => u.WorkItems)
                .HasForeignKey(w => w.AuthorId);

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

            eb.HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.ClientCascade);

        });
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId);
        
        modelBuilder.Entity<WorkState>()
            .HasData(new WorkState{ Id = Guid.NewGuid(), State = "To Do"},
                new WorkState{ Id = Guid.NewGuid(), State = "Doing"},
                new WorkState{ Id = Guid.NewGuid(), State = "Done"});
        modelBuilder.Entity<Tag>()
            .HasData(new Tag
            {
                Id = 1,
                Value = "Web",

            }, new Tag
            {
                Id = 2,
                Value = "UI",

            }, new Tag
            {
                Id = 3,
                Value = "Desktop",

            }, new Tag
            {
                Id = 4,
                Value = "API",

            }, new Tag
            {
                Id = 5,
                Value = "Service",

            });



    }
}