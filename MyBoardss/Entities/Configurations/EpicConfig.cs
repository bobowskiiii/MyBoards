namespace MyBoardss.Entities.Configurations;

public class EpicConfig : IEntityTypeConfiguration<Epic>
{
    public void Configure(EntityTypeBuilder<Epic> builder)
    {
        builder.Property(x => x.EndDate).HasPrecision(3);
    }
}