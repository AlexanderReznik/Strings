using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CsvParserApp.XmlModels
{
    [Serializable]
    public class User
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        public List<int> TaggedMovies { get; set; }

        public List<int> RatedMovies { get; set; }
    }
}
