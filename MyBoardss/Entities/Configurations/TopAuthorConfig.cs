using MyBoardss.Entities.ViewModels;

namespace MyBoardss.Entities.Configurations;

public class TopAuthorConfig : IEntityTypeConfiguration<TopAuthor>
{
    public void Configure(EntityTypeBuilder<TopAuthor> eb)
    {
        eb.ToView("View_TopAuthors");
        eb.HasNoKey();
    }
}