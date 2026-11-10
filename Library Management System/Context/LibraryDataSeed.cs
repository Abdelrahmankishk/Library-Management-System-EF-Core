using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library_Management_System.Context;

internal static class LibraryDataSeed
{
    public static bool SeedData(AppDbContext context)
    {
        try
        {
            context.Database.Migrate();

            var Transaction = context.Database.BeginTransaction();

            bool HasAuthors = context.Authors.Any();
            bool HasCategories = context.Categories.Any();
            bool HasBooks = context.Books.Any();
            bool HasMembers = context.Members.Any();

            if (HasAuthors && HasCategories && HasBooks && HasMembers)
                return false;

            if (!HasAuthors)
            {
                var authors = LoadDataFromJson<Author>("Files/Authors.json");
                context.Authors.AddRange(authors);
            }
            if(!HasCategories)
            {
                var categories = LoadDataFromJson<Category>("Files/Categories.json");
                context.Categories.AddRange(categories);
            }
            context.SaveChanges();
            if (!HasBooks)
            {
                var books = LoadDataFromJson<Book>("Files/Books.json");
                context.Books.AddRange(books);
            }
            if(!HasMembers)
            {
                var members = LoadDataFromJson<Member>("Files/Members.json");
                context.Members.AddRange(members);
            }
            context.SaveChanges();
            Transaction.Commit();
            return true;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            return false;
        }
    }


    private static List<T> LoadDataFromJson<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file was not found.");
        }
        string jsonData = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        return JsonSerializer.Deserialize<List<T>>(jsonData, options) ?? new List<T>();
    }
}
