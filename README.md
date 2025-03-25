# ContactManager.Logic

**Inhalt:**

- Packages installieren
- Entities
  - Entity `EntityObject`
  - Entity `Contact`
- Schnittstellen
  - Schnittstelle `IContext`
- DataContext
    - Klasse `ContactContext`
    - Klasse `Factory` 

## Packages installieren

```csharp
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

	<ItemGroup>
		<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.13" />
		<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.13" />
		<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.13" />
	</ItemGroup>

	<ItemGroup>
    <ProjectReference Include="..\ContactManager.Common\ContactManager.Common.csproj" />
  </ItemGroup>

</Project>
```

## Entities

Erstellen Sie den Ordner `Entities` und fügen Sie die Datei `EntityObject.cs` hinzu.

### Entity `EntityObject`

```csharp
namespace ContactManager.Logic.Entities
{
    /// <summary>
    /// Represents an abstract base class for entities with an identifier.
    /// </summary>
    public abstract class EntityObject : Common.Contracts.IIdentifiable
    {
        /// <summary>
        /// Gets or sets the identifier of the entity.
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        /// <summary>
        /// Copies the properties from another identifiable entity.
        /// </summary>
        /// <param name="other">The other identifiable entity to copy properties from.</param>
        /// <exception cref="ArgumentNullException">Thrown when the other entity is null.</exception>
        public virtual void CopyProperties(Common.Contracts.IIdentifiable other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            Id = other.Id;
        }
    }
}
```

### Entity `Contact`

```csharp
namespace ContactManager.Logic.Entities
{
    /// <summary>
    /// Represents a contact entity.
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("Contacts")]
    [Index(nameof(Email), nameof(PhoneNumber), IsUnique = true)]
    public class Contact : EntityObject, Common.Contracts.IContact
    {
        #region properties
        /// <summary>
        /// Gets or sets the first name of the contact.
        /// </summary>
        [MaxLength(64)]
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the last name of the contact.
        /// </summary>
        [MaxLength(64)]
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the company name of the contact.
        /// </summary>
        [MaxLength(128)]
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email of the contact.
        /// </summary>
        [MaxLength(128)]
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the phone number of the contact.
        /// </summary>
        [MaxLength(32)]
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the address of the contact.
        /// </summary>
        [MaxLength(256)]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the address of the contact.
        /// </summary>
        [MaxLength(1024)]
        public string Note { get; set; } = string.Empty;
        #endregion properties

        #region methods
        /// <summary>
        /// Returns a string representation of the contact.
        /// </summary>
        /// <returns>A string that represents the contact.</returns>
        public override string ToString()
        {
            return $"contact: {Company}{LastName} {FirstName}";
        }
        /// <summary>
        /// Validates the contact's properties.
        /// </summary>
        /// <exception cref="System.Exception">Thrown when validation fails.</exception>
        public void Validate()
        {
            if ((FirstName.Length < 2
                && LastName.Length < 2)
                || Company.Length < 2)
            {
                throw new System.Exception("First name and last name or Company name is required.");
            }
            if (string.IsNullOrEmpty(Email) == false
                && IsValidEmail(Email) == false)
            {
                throw new System.Exception("Email is not valid.");
            }
            if (string.IsNullOrEmpty(PhoneNumber) == false
                && IsValidPhoneNumber(PhoneNumber) == false)
            {
                throw new System.Exception("Phone number is not valid.");
            }
        }

        /// <summary>
        /// Checks if the provided email is valid.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns>True if the email is valid, otherwise false.</returns>
        public static bool IsValidEmail(string email)
        {
            var result = false;

            if (string.IsNullOrWhiteSpace(email) == false)
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                result = Regex.IsMatch(email, pattern);
            }
            return result;
        }

        /// <summary>
        /// Checks if the provided phone number is valid.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns>True if the phone number is valid, otherwise false.</returns>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            var result = false;

            if (string.IsNullOrWhiteSpace(phoneNumber) == false)
            {
                string pattern = @"^\+?[0-9 ]{7,15}$";

                result = Regex.IsMatch(phoneNumber, pattern);
            }
            return result;
        }
        #endregion methods
    }
}
```

## Schnittstellen

Erstellen Sie einen Ordner `Contracts` und fügen Sie die folgenden Schnittstellen hinzu.

### Schnittstelle `IContext`

```csharp
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Logic.Contracts
{
    public interface IContext : IDisposable
    {
        DbSet<Entities.Contact> ContactSet { get; }

        int SaveChanges();
    }
}
```

## DataContext

Erstellen Sie den Ordner `DataContext` und fügen Sie die Klasse `ContactContext.cs` hinzu.

### Klasse `ContactContext`

```csharp
using ContactManager.Logic.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Logic.DataContext
{
    /// <summary>
    /// Represents the database context for managing contacts.
    /// </summary>
    internal partial class ContactContext : DbContext, IContext
    {
        #region fields
        /// <summary>
        /// The type of the database (e.g., Sqlite, SqlServer).
        /// </summary>
        private static string DatabaseType = "Sqlite";

        /// <summary>
        /// The connection string for the database.
        /// </summary>
        private static string ConnectionString = "data source=CompanyManager.db";
        #endregion fields

        /// <summary>
        /// Initializes static members of the <see cref="ContactContext"/> class.
        /// </summary>
        static ContactContext()
        {
            var appSettings = Common.Modules.Configuration.AppSettings.Instance;

            DatabaseType = appSettings["Database:Type"] ?? DatabaseType;
            ConnectionString = appSettings[$"ConnectionStrings:{DatabaseType}ConnectionString"] ?? ConnectionString;
        }

        #region properties
        /// <summary>
        /// Gets or sets the DbSet for contacts.
        /// </summary>
        public DbSet<Entities.Contact> ContactSet { get; set; }
        #endregion properties

        /// <summary>
        /// Configures the database context options.
        /// </summary>
        /// <param name="optionsBuilder">The options builder to be used for configuration.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (DatabaseType == "Sqlite")
            {
                optionsBuilder.UseSqlite(ConnectionString);
            }
            else if (DatabaseType == "SqlServer")
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
```
### Klasse `Factory`

```csharp
using ContactManager.Logic.Contracts;

namespace ContactManager.Logic.DataContext
{
    /// <summary>
    /// Factory class to create instances of IMusicStoreContext.
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// Creates an instance of IContext.
        /// </summary>
        /// <returns>An instance of IContext.</returns>
        public static IContext CreateContext()
        {
            var result = new ContactContext();

            return result;
        }

#if DEBUG
        public static void CreateDatabase()
        {
            var context = new ContactContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public static void InitDatabase()
        {
            var context = CreateContext();

            CreateDatabase();
        }
#endif
    }
}
```

**Viel Spaß!**