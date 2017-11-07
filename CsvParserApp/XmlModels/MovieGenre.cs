using System;
using System.Xml.Serialization;

namespace CsvParserApp.XmlModels
{
    [Serializable]
    public class MovieGenre
    {
        [XmlAttribute]
        public string Name { get; set; }
    }
}
