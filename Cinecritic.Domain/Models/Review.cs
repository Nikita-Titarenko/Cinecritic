using System.ComponentModel.DataAnnotations;

namespace Cinecritic.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string ReviewText { get; set; } = string.Empty;

        public DateTime ReviewDateTime { get; set; }

        public int MovieUserId { get; set; }

        public MovieUser MovieUser { get; set; } = new MovieUser();
    }
}
