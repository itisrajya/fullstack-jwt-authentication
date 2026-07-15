using System;
using System.ComponentModel.DataAnnotations;

namespace server.authentication.data.Entities
{
    public class PasswordResetToken
    {
        [Key]
        public int PasswordResetTokenId { get; set; }

        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// SHA-256 hash of the raw token. The raw token is never persisted.
        /// </summary>
        [Required]
        public string TokenHash { get; set; }

        [Required]
        public DateTime CreatedAtUtc { get; set; }

        [Required]
        public DateTime ExpiresAtUtc { get; set; }

        public DateTime? UsedAtUtc { get; set; }
    }
}
