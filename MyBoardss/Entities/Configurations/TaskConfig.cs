namespace MyBoardss.Entities.Configurations;

public class TaskConfig : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.Property(x => x.Activity).HasMaxLength(200);
        builder.Property(x => x.RemaningWork).HasPrecision(14, 2);
    }
}