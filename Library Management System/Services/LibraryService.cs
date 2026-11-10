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
}
