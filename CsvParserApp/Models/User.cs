using System.Collections.Generic;

namespace CsvParserApp.Models
{
    public class User
    {
        public User()
        {
            Tags = new List<UserTag>();
            Ratings = new List<UserRating>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<UserTag> Tags { get; }

        public List<UserRating> Ratings { get; }
    }
}
