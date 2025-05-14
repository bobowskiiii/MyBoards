namespace MyBoardss.Entities.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(a => a.UserId);
    }
}