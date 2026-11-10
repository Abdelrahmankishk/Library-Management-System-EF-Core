using Library_Management_System.Context;
using Library_Management_System.Services;

namespace Library_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Library Management System - EF Core");
            var dbContext = new AppDbContext();
            dbContext.Database.EnsureCreated();

            bool Result = LibraryDataSeed.SeedData(dbContext);
            if(Result)
            {
                Console.WriteLine("Data seeded successfully.");
            }
            else
            {
                Console.WriteLine("Data seeding failed or already exists.");
            }

            #region Retrieve the book title, its category title , and the author’s full name for all books whose price is greater than 300.
            LibraryService.RetrieveBooksGeaterThanPrice(dbContext, 300);
            #endregion
        }
    }
}
