using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Configurations
{
    public class MemberConfigurations : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(X => X.Name)
      .HasColumnType("varchar")
      .HasMaxLength(50);

            builder.Property(X => X.Email)
                  .HasColumnType("varchar")
                  .HasMaxLength(100);

            builder.Property(X => X.PhoneNumber)
                  .HasColumnType("varchar")
                  .HasMaxLength(11);


            builder.Property(X => X.Address)
                  .HasColumnType("varchar")
                  .HasMaxLength(100);

            builder.Property(X => X.MembershipDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(X => X.Status)
                   .HasConversion<string>()         // store "Active"/"Suspended", not 0/1
                   .HasColumnType("varchar")
                   .HasMaxLength(9);

            builder.ToTable(Tb =>
            {
                // e-mail shape: something@something.something, no spaces
                Tb.HasCheckConstraint("CK_Members_Email",
                "[Email] LIKE '_%@_%._%' AND [Email] NOT LIKE '% %'");

                // Egyptian mobile: 01[0|1|2|5] followed by 8 digits (11 digits total)
                Tb.HasCheckConstraint("CK_Members_PhoneNumber",
                    "[PhoneNumber] LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
            });

        }
    }
}
