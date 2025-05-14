namespace MyBoardss.Entities.Configurations;

public class IssueConfig : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.Property(x => x.Efford).HasColumnType("decimal(5,2)");
    }
}   