//@CodeCopy
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Logic.Entities
{
    /// <summary>
    /// Represents a contact entity.
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("Contacts")]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
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
        #endregion methods
    }
}
