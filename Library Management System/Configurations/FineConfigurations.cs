using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Configurations;

public class FineConfigurations : IEntityTypeConfiguration<Fine>
{
    public void Configure(EntityTypeBuilder<Fine> builder)
    {
        builder.Property(f => f.Amount)
            .HasPrecision(6, 2);

        builder.Property(f => f.IssuedDate)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(X => X.Status)
                   .HasConversion<string>()         // store Pending,Paid not 0/1
                   .HasColumnType("varchar")
                   .HasMaxLength(7);
    }
}
