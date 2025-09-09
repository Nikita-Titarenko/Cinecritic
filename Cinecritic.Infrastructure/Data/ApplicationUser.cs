using System.ComponentModel.DataAnnotations;
using Cinecritic.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Cinecritic.Infrastructure.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [StringLength(30)]
        public string DisplayName { get; set; } = string.Empty;
        public IEnumerable<MovieUser> MovieUsers { get; set; } = new List<MovieUser>();
        public IEnumerable<WatchList> WatchLists { get; set; } = new List<WatchList>();
    }

}
