namespace MyBoardss.Entities.Configurations;

public class WorkItemConfig : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> eb)
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
    }
}