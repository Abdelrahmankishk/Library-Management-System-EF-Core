using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Configurations;

public class AuthorConfigurations : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(X => X.FirstName)
                  .HasColumnType("varchar")
                  .HasMaxLength(20);

        builder.Property(X => X.LastName)
               .HasColumnType("varchar")
               .HasMaxLength(20);

        #region Author-Book Relationship
        builder.HasMany(x => x.Books)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);
        #endregion
    }
}
