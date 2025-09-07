using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Cinecritic.Infrastructure.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [StringLength(30)]
        public string DisplayName { get; set; } = string.Empty;
    }

}
