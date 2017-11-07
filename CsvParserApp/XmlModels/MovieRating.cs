using System;

namespace CsvParserApp.XmlModels
{
    [Serializable]
    public class MovieRating
    {
        public int UserId { get; set; }

        public double Rating { get; set; }

        public DateTime RateDate { get; set; }
    }
}
