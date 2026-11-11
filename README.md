# 📚 Library Management System — EF Core

A **C# console-based Library Management System** built using **Entity Framework Core (EF Core)** and **SQL Server**. The application models the core library operations, including books, categories, authors, members, borrowing transactions, and fines.

The system follows an **EF Core Code-First approach**, using Fluent API configurations, database migrations, and JSON-based data seeding to establish and maintain the database. It implements borrowing and returning operations, automatic overdue fine calculation, and LINQ queries for retrieving related library data.

## ✨ Project Highlights

- 📚 Manage books, authors, categories, and registered library members.
- 🔄 Implement book borrowing and returning operations with availability tracking.
- 💰 Automatically calculate and issue fines for overdue book returns.
- 👤 Manage member account status based on borrowing eligibility and overdue fines.
- 🔗 Model borrowing records using a composite-key `MemberLoan` entity.
- 🌱 **JSON-Based Data Seeding:** Deserialize external JSON files into entity objects using a reusable generic deserialization method.
- ⚙️ **Assembly-Based Configuration Discovery:** Automatically discover and apply all entity configurations using `ApplyConfigurationsFromAssembly()` instead of registering each configuration manually.
- 🔤 **String-Based Enum Conversion:** Store enum labels such as `Active`, `Borrowed`, and `Pending` in SQL Server instead of their underlying numeric values.
- 🔒 Use database transactions to maintain consistency during seeding, borrowing, and returning operations.
- 🛡️ Define database-level check constraints for publication years, available copies, email formats, and Egyptian phone numbers.
- 🗄️ Use EF Core Code-First with SQL Server and database migrations.
- 🧩 Separate entity models, configurations, database context, data seeding, and business operations into dedicated folders.

## 🧭 System Overview

The application provides a set of library management operations through its `LibraryService` class.

| **Operation** | **Description** |
|---|---|
| 📖 Book Retrieval | Retrieve books with their categories and authors when their prices exceed a specified amount. |
| ✍️ Author Management | Retrieve all authors alongside their books, including authors without any associated books. |
| 📥 Borrowing | Borrow a book after validating member status, book availability, and existing active loans. |
| 📤 Returning | Register book returns, update availability, and calculate fines when the return is overdue. |
| 📋 Active Loans | Retrieve members who currently have books that have not yet been returned. |

## 🏗️ Database Design and Relationships

The database represents the main entities of the library management system and their relationships.

### 🧩 Main Entities

| **Entity** | **Responsibility** |
|---|---|
| `Author` | Stores author information, including first name, last name, and date of birth. |
| `Category` | Represents book categories and their descriptions. |
| `Book` | Stores book details, including title, price, publication year, and copy availability. |
| `Member` | Stores registered member information, contact details, membership date, and account status. |
| `Loan` | Represents a borrowing transaction and its current status. |
| `MemberLoan` | Represents the association between a member, a loan, and a book, including due and return dates. |
| `Fine` | Stores fine amounts, issuance dates, payment dates, and payment status. |

### 🔗 Entity Relationships

- **Author–Book (One-to-Many):** An author can have multiple books, while each book belongs to one author.
- **Category–Book (One-to-Many):** A category can contain multiple books, while each book belongs to one category.
- **Member–MemberLoan (One-to-Many):** A member can have multiple borrowing records.
- **Book–MemberLoan (One-to-Many):** A book can appear in multiple borrowing records over time.
- **Loan–MemberLoan (One-to-Many):** A loan is associated with its borrowing record or records.
- **Loan–Fine (One-to-One):** Each loan can have one associated fine.

The `MemberLoan` entity acts as the junction entity between `Member`, `Loan`, and `Book`. It uses a **composite primary key** consisting of `LoanId`, `BookId`, and `MemberId`, while maintaining the borrowing and returning dates.

## 📜 Key Business Rules

The system implements the following rules across its data model and service operations.

### 📚 Book Management

- Each book belongs to an existing author and category.
- Book prices are stored with two decimal places.
- Publication years must be between 1950 and the current year.
- Available copies cannot exceed the total number of copies.
- A book must have available copies before it can be borrowed.

### 👤 Member Management

- Each member has a membership date and account status.
- Supported member statuses are `Active` and `Suspended`.
- Only active members can borrow books.
- A member cannot borrow another book while an existing borrowing record remains active.

### 📥 Borrowing Operations

- Each borrowing operation creates a new `Loan` and an associated `MemberLoan` record.
- The due date is calculated according to the specified borrowing period.
- Available book copies are decremented after a successful borrowing operation.
- Borrowing operations are wrapped in database transactions.

### 📤 Returning Operations

- The actual return date is recorded in the `MemberLoan` entity.
- Available book copies are incremented after a successful return.
- If a book is returned on or before its due date, no fine is issued.
- If a book is returned after its due date, the system calculates and issues an overdue fine.
- The loan status is updated according to the return result.

## ⚙️ Entity Framework Core Implementation

The project uses several EF Core features to organize database configuration, maintain data integrity, and simplify database operations.

### 🧩 Assembly-Based Entity Configuration

Each entity has a dedicated configuration class implementing `IEntityTypeConfiguration<T>`.

Instead of manually registering every configuration inside `OnModelCreating()`, the application uses assembly scanning:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyConfigurationsFromAssembly(
        Assembly.GetExecutingAssembly()
    );
}
```

This automatically discovers and applies the entity configurations within the executing assembly, making the configuration process easier to maintain as the project grows.

### 🔤 Enum-to-String Conversion

The project uses EF Core's `HasConversion<string>()` to store enum labels in the database instead of their default numeric values.

For example:

```csharp
builder.Property(x => x.Status)
       .HasConversion<string>()
       .HasColumnType("varchar")
       .HasMaxLength(9);
```

This configuration is applied to:

- `MemberStatus`
- `LoanStatus`
- `FineStatus`

As a result, the database stores readable values such as `Active`, `Suspended`, `Borrowed`, `Returned`, and `Pending` instead of numeric representations.

### 🌱 JSON-Based Data Seeding

One of the main features of this project is its JSON-based data seeding implementation.

The `LibraryDataSeed` class uses `System.Text.Json` to deserialize external JSON files into strongly typed entity collections.

A reusable generic method handles the deserialization:

```csharp
private static List<T> LoadDataFromJson<T>(string filePath)
{
    if (!File.Exists(filePath))
        throw new FileNotFoundException("The file was not found.");

    string jsonData = File.ReadAllText(filePath);

    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    options.Converters.Add(new JsonStringEnumConverter());

    return JsonSerializer.Deserialize<List<T>>(jsonData, options)
           ?? new List<T>();
}
```

The implementation provides:

- Generic JSON deserialization for different entity types.
- Case-insensitive JSON property matching.
- Support for enum values represented as strings through `JsonStringEnumConverter`.
- Database transaction handling during the seeding process.
- Database migration execution before inserting seed data.

The following datasets are expected:

- `Authors.json`
- `Categories.json`
- `Books.json`
- `Members.json`

This approach separates initial data from the application code and makes it possible to manage seed records through external JSON files.

### 🔒 Database Transactions

Database transactions are used in important operations to maintain consistency.

- **Data Seeding:** Groups the initial data insertion operations within a database transaction.
- **Borrowing:** Ensures that loan creation, borrowing record insertion, and available-copy updates are handled together.
- **Returning:** Groups return-date updates, available-copy restoration, fine issuance, and status changes into one transaction.

This helps prevent partially completed database operations when an error occurs.

### 🔄 Database Migrations

The project follows the EF Core Code-First workflow.

The `Migrations` folder contains the migration history and model snapshot used to create and maintain the database schema.

The JSON seeding routine also calls `Database.Migrate()` before inserting the initial data, allowing pending migrations to be applied as part of the seeding process.


## 🗂️ Project Structure

The project follows a single-project architecture with responsibilities separated into dedicated folders.

```text
Library-Management-System-EF-Core/
│
├── Library Management System.sln
│
└── Library Management System/
    │
    ├── Configurations/
    │   ├── AuthorConfigurations.cs
    │   ├── BookConfigurations.cs
    │   ├── CategoryConfigurations.cs
    │   ├── FineConfigurations.cs
    │   ├── LoanConfigurations.cs
    │   ├── MemberConfigurations.cs
    │   └── MemberLoanConfiguration.cs
    │
    ├── Context/
    │   ├── AppDbContext.cs
    │   └── LibraryDataSeed.cs
    │
    ├── Migrations/
    │   ├── [EF Core migration files]
    │   └── AppDbContextModelSnapshot.cs
    │
    ├── Models/
    │   ├── Enums/
    │   │   ├── FineStatus.cs
    │   │   ├── LoanStatus.cs
    │   │   └── MemberStatus.cs
    │   │
    │   ├── Author.cs
    │   ├── BaseEntity.cs
    │   ├── Book.cs
    │   ├── Category.cs
    │   ├── Fine.cs
    │   ├── Loan.cs
    │   ├── Member.cs
    │   └── MemberLoan.cs
    │
    ├── Services/
    │   └── LibraryService.cs
    │
    ├── ClassDiagram1.cd
    ├── Program.cs
    └── Library Management System.csproj
```

### 📁 Folder Responsibilities

| **Folder / File** | **Responsibility** |
|---|---|
| `Models` | Contains the database entities and enumerations. |
| `Configurations` | Defines entity mappings, relationships, property constraints, and database configurations. |
| `Context` | Contains the application DbContext and JSON-based data seeding implementation. |
| `Migrations` | Contains database schema changes generated by EF Core and the model snapshot. |
| `Services` | Implements book retrieval, borrowing, returning, fine generation, and active-loan queries. |
| `Files` | Holds the external JSON datasets used for initial data seeding. |
| `Program.cs` | Initializes the DbContext and invokes the required application operations. |

## 🔄 Application Flow

```text
                   Program.cs
                       │
                       ▼
                  AppDbContext
                       │
                       ▼
              LibraryDataSeed
                       │
              Apply Migrations
                       │
              Deserialize JSON
                       │
                       ▼
                 LibraryService
                       │
                       ▼
                  EF Core ORM
                       │
                       ▼
                   SQL Server
```

## 🛠️ Technologies

- **C#**
- **.NET 9**
- **Entity Framework Core 9**
- **SQL Server**
- **Fluent API**
