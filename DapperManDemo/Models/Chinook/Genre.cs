namespace DapperManDemo.Models.Chinook
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{GenreId}\t{Name}";
        }
    }
}
