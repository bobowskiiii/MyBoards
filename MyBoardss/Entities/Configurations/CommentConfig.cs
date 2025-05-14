namespace MyBoardss.Entities.Configurations;

public class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> eb)
    {
        eb.Property(x => x.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();

        eb.HasOne(c => c.Author)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}