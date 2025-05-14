using MyBoardss.Entities.Configurations;
using MyBoardss.Entities.ViewModels;

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
    public DbSet<TopAuthor> ViewTopAuthors { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // new AddressConfig().Configure(modelBuilder.Entity<Address>());
        // Konfiguracja encji poprzez IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}