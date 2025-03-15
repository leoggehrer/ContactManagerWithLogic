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
namespace ContactManager.Common.Contracts
{
    /// <summary>
    /// Represents an contact in the contact manager.
    /// </summary>
    public interface IContact : IIdentifiable
    {
        #region Properties
        /// <summary>
        /// Gets or sets the first name of the contact.
        /// </summary>
        string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name of the contact.
        /// </summary>
        string LastName { get; set; }
        /// <summary>
        /// Gets or sets the contact name of the contact.
        /// </summary>
        string contact { get; set; }
        /// <summary>
        /// Gets or sets email of the contact.
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// Gets or sets phone number of the contact.
        /// </summary>
        string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets address of the contact.
        /// </summary>
        string Address { get; set; }
        /// <summary>
        /// Gets or sets note of the contact.
        /// </summary>
        string Note { get; set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Copies the properties of the specified contact to this contact.
        /// </summary>
        /// <param name="other">The contact object that is copied.</param>
        void CopyProperties(IContact other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            Id = other.Id;
            FirstName = other.FirstName;
            LastName = other.LastName;
            contact = other.contact;

            Email = other.Email;
            PhoneNumber = other.PhoneNumber;
            Address = other.Address;
            Note = other.Note;
        }
        #endregion Methods
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