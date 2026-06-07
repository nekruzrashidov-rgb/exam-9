namespace Infrastructure.Persistense.Confidurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BorrowConfiguration : IEntityTypeConfiguration<Borrow>
{
    public void Configure(EntityTypeBuilder<Borrow> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BorrowDate)
            .IsRequired();

        builder.Property(x => x.ReturnDate)
            .IsRequired(false);


        builder.HasOne(x => x.Book)
               .WithMany(b => b.Borrows)           
               .HasForeignKey(x => x.BookId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Member)
               .WithMany(x => x.Borrows)
               .HasForeignKey(x => x.MemberId)
               .OnDelete(DeleteBehavior.Restrict);



        builder.HasIndex(x => x.BookId);
        builder.HasIndex(x => x.MemberId);
    }
}