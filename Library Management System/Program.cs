namespace Library_Management_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Library Management System - EF Core");
            var dbContext = new AppDbContext();
            dbContext.Database.EnsureCreated();
        }
    }
}
