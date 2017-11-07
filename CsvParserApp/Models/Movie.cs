using System.Collections.Generic;

namespace CsvParserApp.Models
{
    public class Movie
    {
        public Movie()
        {
            Genres = new List<Genre>();
            Tags = new List<UserTag>();
            Ratings = new List<UserRating>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public List<Genre> Genres { get; }

        public List<UserTag> Tags { get; }

        public List<UserRating> Ratings { get; }
    }
}
