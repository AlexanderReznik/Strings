using System;
using System.Xml.Serialization;

namespace CsvParserApp.XmlModels
{
    [Serializable]
    public class MovieRating
    {
        [XmlAttribute]
        public int UserId { get; set; }

        [XmlAttribute]
        public double Rating { get; set; }

        public DateTime RateDate { get; set; }
    }
}
