using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library_Management_System.Configurations;

public class MemberLoanConfiguration : IEntityTypeConfiguration<MemberLoan>
{
    public void Configure(EntityTypeBuilder<MemberLoan> builder)
    {
        builder.HasKey(x => new { x.LoanId, x.BookId, x.MemberId });
    }
}
