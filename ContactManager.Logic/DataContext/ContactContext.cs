//@CodeCopy
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
