using Library_Management_System.Context;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Services;

public static class LibraryService
{
    public static void RetrieveBooksGeaterThanPrice(AppDbContext context, decimal price)
    {
        var books = context.Books.Where(b => b.Price > price)
            // NO NEED TO INCLUDE NAVIGATION PROPERTIES WHEN USING SELECT TO PROJECT SPECIFIC FIELDS 
            // When Querying Full Entities (Where .Include() is required)
            //.Include(b => b.Category)
            //.Include(b => b.Author)                           
            .Select(b => new
            {
                Title = b.Title,
                Price = b.Price,
                CategoryName = b.Category.Title,
                AuthorName = (b.Author.FirstName ?? "") + " " + (b.Author.LastName ?? "")
            })
            .ToList();
        
        if (books.Count == 0)
        {
            Console.WriteLine($"No books found with price greater than {price} EGP.");
        }
        else
        {
            Console.WriteLine($"Books with price greater than {price} EGP:");
            foreach (var book in books)
            {
                Console.WriteLine($"    Title: {book.Title}, Price: {book.Price} EGP, Category: {book.CategoryName}, Author: {book.AuthorName}");
            }
        }
    }

    public static void RetrieveAllAuthorsAndBooks(AppDbContext context)
    {
        var authors = context.Authors
            .Include(a => a.Books)
            .ToList();
        foreach (var author in authors)
        {
            Console.WriteLine($"Author: {author.FirstName} {author.LastName}, DOB: {author.DateOfBirth.ToShortDateString()}");
            if (author.Books.Count == 0)
            {
                Console.WriteLine("    No books found for this author.");
                continue;
            }
            foreach (var book in author.Books)
            {
                Console.WriteLine($"    Book: {book.Title}, Price: {book.Price} EGP, Available Copies: {book.AvailableCopies}, Total Copies: {book.TotalCopies}, Published Year: {book.PublicationYear}");
            }
        }
    }


    public static (bool Success, string Message) BorrowBook(AppDbContext context, int memberId, int bookId, int daysToReturn)
    {
        var member = context.Members.Find(memberId);
        var book = context.Books.Find(bookId);
        if (member is null || book is null)
        {
            return (false, "Member or Book not found.");
        }
        if(member.Status != Models.Enums.MemberStatus.Active)
        {
            return (false, "Member is Suspended");
        }
        if (book.AvailableCopies <= 0)
        {
            return (false, "No available copies of the book to borrow.");
        }

        var Loan = new Loan();
        context.Loans.Add(Loan);

        var memberLoan = new MemberLoan
        {
            MemberId = memberId,
            Loan = Loan,
            BookId = bookId,
            DueDate = DateTime.Now.AddDays(daysToReturn)
        };
        context.MembersLoans.Add(memberLoan);
        book.AvailableCopies--;

        //    context.SaveChanges();
        //    return (true, "Book borrowed successfully.");
        //
        var savedEntities = context.SaveChanges();

        var savedMemberLoan = context.MembersLoans
            .AsNoTracking()
            .SingleOrDefault(x =>
                x.LoanId == memberLoan.LoanId &&
                x.BookId == memberLoan.BookId &&
                x.MemberId == memberLoan.MemberId);

        Console.WriteLine($"Saved entities: {savedEntities}");
        Console.WriteLine($"Generated Loan ID: {memberLoan.LoanId}");
        Console.WriteLine($"MemberLoan exists: {savedMemberLoan is not null}");

        return savedMemberLoan is not null
            ? (true, "Book borrowed successfully.")
            : (false, "SaveChanges completed, but the MemberLoan row was not found.");
    }
}
