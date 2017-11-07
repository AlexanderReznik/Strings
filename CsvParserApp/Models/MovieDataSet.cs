using System.Collections.Generic;

namespace CsvParserApp.Models
{
    public class MovieDataSet
    {
        public MovieDataSet()
        {
            Movies = new List<Movie>();
            Genres = new List<Genre>();
            Users = new List<User>();
        }

        public List<Movie> Movies { get; }

        public List<Genre> Genres { get; }

        public List<User> Users { get; }
    }
}
