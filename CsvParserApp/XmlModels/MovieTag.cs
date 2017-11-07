using System;
using System.Xml.Serialization;

namespace CsvParserApp.XmlModels
{
    [Serializable]
    public class MovieTag
    {
        [XmlAttribute]
        public int UserId { get; set; }

        public string Tag { get; set; }

        public DateTime TagDate { get; set; }
    }
}
