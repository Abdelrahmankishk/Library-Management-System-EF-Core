using Library_Management_System.Context;
using Library_Management_System.Models;
using Library_Management_System.Services;

namespace Library_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Library Management System - EF Core");
            var dbContext = new AppDbContext();

            //bool Result = LibraryDataSeed.SeedData(dbContext);
            //if (Result)
            //{
            //    Console.WriteLine("Data seeded successfully.");
            //}
            //else
            //{
            //    Console.WriteLine("Data seeding failed or already exists.");
            //}

            #region Retrieve the book title, its category title , and the author’s full name for all books whose price is greater than 300.
            //LibraryService.RetrieveBooksGeaterThanPrice(dbContext, 300);
            #endregion

            #region Retrieve All Authors And His/Her Books if Exists
            // LibraryService.RetrieveAllAuthorsAndBooks(dbContext);
            #endregion

            #region Member with id 1 Want To Borrow The Book With Id 2 And He Will Return it After 5 Days
            var result = LibraryService.BorrowBook(dbContext, 6, 6, 5);
            if (result.Success)
            {
                Console.WriteLine(result.Message);
            }
            else
            {
                Console.WriteLine($"Failed to borrow book: {result.Message}");
            }
            #endregion
        }


    }
}
