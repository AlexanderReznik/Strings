using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CsvParserApp.XmlModels
{
    [Serializable]
    public class Movie
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public int Year { get; set; }

        public List<MovieGenre> Genres { get; set; }

        public List<MovieTag> Tags { get; set; }

        public List<MovieRating> Ratings { get; set; }
    }
}
