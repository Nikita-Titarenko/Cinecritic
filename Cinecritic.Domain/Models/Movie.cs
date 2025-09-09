using System.ComponentModel.DataAnnotations;

namespace Cinecritic.Domain.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(2000)]
        public string? Description { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public MovieType MovieType { get; set; } = default!;
        public int MovieTypeId {  get; set; }
        public IEnumerable<MovieUser> MovieUsers { get; set; } = new List<MovieUser>();
        public IEnumerable<WatchList> WatchLists { get; set; } = new List<WatchList>();
    }
}