using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models;

public class Book : BaseEntity
{
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public int PublicationYear { get; set; }
    public int AvailableCopies { get; set; }
    public int TotalCopies { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; } = default!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;

    public ICollection<MemberLoan> MemberLoans { get; set; } = new HashSet<MemberLoan>();
}
