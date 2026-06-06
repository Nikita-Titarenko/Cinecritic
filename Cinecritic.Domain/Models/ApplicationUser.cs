using System.ComponentModel.DataAnnotations;
using Cinecritic.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Cinecritic.Infrastructure.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        [StringLength(30)]
        public string DisplayName { get; set; } = string.Empty;
        public DateTime CreationDateTime { get; set; }
        public bool IsCenturion { get; set; }
        public IEnumerable<MovieUser> MovieUsers { get; set; } = new List<MovieUser>();
        public IEnumerable<WatchList> WatchLists { get; set; } = new List<WatchList>();
    }
}
