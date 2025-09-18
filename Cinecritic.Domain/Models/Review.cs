using System.ComponentModel.DataAnnotations;

namespace Cinecritic.Domain.Models
{
    public class Review
    {

        [StringLength(500)]
        public string ReviewText { get; set; } = string.Empty;

        public DateTime ReviewDateTime { get; set; }

        public MovieUser MovieUser { get; set; } = new MovieUser();

        public int MovieId { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}
