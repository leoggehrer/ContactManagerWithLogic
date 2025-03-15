//@CodeCopy
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
