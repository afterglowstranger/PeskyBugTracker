using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PeskyBugTracker.Models
{
    public class AuthenticateModel
    {

        [NotMapped]
        [Required]
        public string Username { get; set; }

        [NotMapped]
        [Required]
        public string Password { get; set; }
    }
}
