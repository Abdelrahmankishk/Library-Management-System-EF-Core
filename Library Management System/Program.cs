using Library_Management_System.Context;

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
        }
    }
}
