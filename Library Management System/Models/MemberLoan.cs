using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models;

public class MemberLoan
{
    public int LoanId { get; set; }
    public Loan Loan { get; set; } = default!;
    public int BookId { get; set; }
    public Book Book { get; set; } = default!;
    public int MemberId { get; set; }
    public Member Member { get; set; } = default!;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
