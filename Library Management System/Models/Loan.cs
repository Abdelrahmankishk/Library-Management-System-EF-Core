using Library_Management_System.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models;

public class Loan : BaseEntity
{
    public DateTime LoanDate { get; set; }
    public LoanStatus Status { get; set; }
}
