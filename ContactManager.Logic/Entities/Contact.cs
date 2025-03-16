//@CodeCopy
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
