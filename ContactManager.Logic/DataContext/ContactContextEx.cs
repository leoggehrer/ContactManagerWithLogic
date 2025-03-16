using Microsoft.EntityFrameworkCore;

namespace ContactManager.Logic.DataContext
{
    /// <summary>
    /// Partial class for ContactContext to override SaveChanges and validate entities.
    /// </summary>
    partial class ContactContext
    {
        /// <summary>
        /// Overrides the SaveChanges method to validate entities before saving.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            ValidateEntities();
            return base.SaveChanges();
        }

        /// <summary>
        /// Validates entities that are being added or modified.
        /// </summary>
        private void ValidateEntities()
        {
            var entities = ChangeTracker.Entries()
                                        .Where(e => e.State == EntityState.Added
                                                 || e.State == EntityState.Modified)
                                        .Select(e => e.Entity);

            var errors = new List<string>();

            foreach (var entity in entities)
            {
                if (entity is Entities.Contact contact)
                {
                    contact.Validate();
                }
            }
        }
    }
}
