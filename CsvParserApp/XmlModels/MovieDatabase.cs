using System;
using System.Collections.Generic;

namespace CsvParserApp.XmlModels
{
    [Serializable]
    public class MovieDatabase
    {
        public List<Movie> Movies { get; set; }

        public List<User> Users { get; set; }
    }
}
