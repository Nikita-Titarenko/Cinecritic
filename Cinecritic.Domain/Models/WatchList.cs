using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinecritic.Domain.Models
{
    public class WatchList
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; } = default!;

        public string UserId { get; set; } = string.Empty;
    }
}
