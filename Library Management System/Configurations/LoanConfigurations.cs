using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Configurations;

public class LoanConfigurations : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.Property(b => b.LoanDate)
            .HasDefaultValueSql("getdate()");

        builder.Property(X => X.Status)
                   .HasConversion<string>()         // store Borrowed,Returned,Overdue, not 0/1
                   .HasColumnType("varchar")
                   .HasMaxLength(8);

        builder.HasMany(l => l.MemberLoans)
            .WithOne(ml => ml.Loan)
            .HasForeignKey(ml => ml.LoanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
