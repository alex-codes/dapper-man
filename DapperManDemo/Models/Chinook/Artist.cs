using System.Collections.Generic;
using System.Linq;

namespace DapperManDemo.Models.Chinook
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public List<Album> Albums { get; set; }

        public override string ToString()
        {
            return $"{Name}\t{(Albums != null ? string.Join(", ", Albums.Select(a => a.Title)) : "N/A")}";
        }
    }
}
