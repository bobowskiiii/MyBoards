namespace MyBoardss.Entities.Configurations;

public class WorkStateConfig : IEntityTypeConfiguration<WorkState>
{
    public void Configure(EntityTypeBuilder<WorkState> builder)
    {
        builder.Property(s => s.State)
            .IsRequired()
            .HasMaxLength(60);
        
        builder.HasData(new WorkState{ Id = Guid.NewGuid(), State = "To Do"},
            new WorkState{ Id = Guid.NewGuid(), State = "Doing"},
            new WorkState{ Id = Guid.NewGuid(), State = "Done"});
    }
}