using System;

namespace CsvParserApp.XmlModels
{
    [Serializable]
    public class MovieTag
    {
        public int UserId { get; set; }

        public string Tag { get; set; }

        public DateTime TagDate { get; set; }
    }
}
