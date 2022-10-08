using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace UserManagementWithIdentity.Models
{
    // to add new properity to User identity table 
    public class ApplicationUser:IdentityUser
    {

        [Required]
        [MaxLength(100)]
        public string firstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string lastName { get; set; }
        
        public byte[]? profilePricture{ get; set; } // to save phone as binary data in database

    }
}
