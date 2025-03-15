//@CodeCopy
namespace ContactManager.Common.Contracts
{
    /// <summary>
    /// Represents an identifiable in the contact manager.
    /// </summary>
    public interface IIdentifiable
    {
        #region Properties
        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        int Id { get; protected set; }
        #endregion Properties
    }
}
