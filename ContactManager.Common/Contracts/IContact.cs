//@CodeCopy
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
        /// Gets or sets the company name of the contact.
        /// </summary>
        string Company { get; set; }
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
            Company = other.Company;

            Email = other.Email;
            PhoneNumber = other.PhoneNumber;
            Address = other.Address;
            Note = other.Note;
        }
        #endregion Methods
    }
}
