using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Configurations;

public class BookConfigurations : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
               .HasColumnType("varchar")
               .HasMaxLength(50);                 

        builder.Property(b => b.Price)
               .HasPrecision(6, 2);                     

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Books_PublicationYear",
                "[PublicationYear] >= 1950 AND [PublicationYear] <= YEAR(GETDATE())");
            t.HasCheckConstraint("CK_Books_AvailableCopies",
                "[AvailableCopies] >= 0 AND [AvailableCopies] <= [TotalCopies]");
        });

        #region Category Book Relationship
        builder.HasOne(b => b.Category)
               .WithMany(c => c.Books)
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
        #endregion

        builder.HasMany(b => b.MemberLoans)
               .WithOne(ml => ml.Book)
               .HasForeignKey(ml => ml.BookId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
