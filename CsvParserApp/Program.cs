using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CsvParserApp.CsvModels;
using CsvParserApp.Models;
using CsvParserApp.XmlModels;

namespace CsvParserApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NOTE 1. Download MovieLens latest small dataset from https://grouplens.org/datasets/movielens/, extract csv files and copy them to the current bin folder.
            // NOTE 2. Open project properties, go on Debug tab and add Command line arguments there "--movies=movies.csv --ratings=ratings.csv --tags=tags.csv --output=database.xml".

            var moviesPath = GetFilePath(args, "movies");
            var ratingsPath = GetFilePath(args, "ratings");
            var tagsPath = GetFilePath(args, "tags");
            var output = GetFilePath(args, "output");

            var moviesDataSet = LoadMovieDataSet(moviesPath, ratingsPath, tagsPath, new MovieDataSetBuilder());

            Console.WriteLine("Movies in data set: {0}", moviesDataSet.Movies.Count);
            Console.WriteLine("Genres in data set: {0}", moviesDataSet.Genres.Count);
            Console.WriteLine("Users in data set: {0}", moviesDataSet.Users.Count);

            SaveToXml(moviesDataSet, output);

            Console.WriteLine("\nPress enter key to exit.");
            Console.ReadLine();
        }

        public static string GetFilePath(string[] args, string expectedParameter)
        {
            foreach (var arg in args)
            {
                string[] array;

                array = arg.Trim('-').Split('=');
                
                string parameterName = array[0];

                if (parameterName == expectedParameter)
                {
                    return array[1];
                }
            }

            throw new Exception(string.Format("expectedParameter {0} is not found.", expectedParameter));
        }

        private static MovieDataSet LoadMovieDataSet(string moviesPath, string ratingsPath, string tagsPath, IMovieDataSetBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (File.Exists(moviesPath) == false)
            {
                throw new Exception(string.Format("file {0} is not exists.", moviesPath));
            }

            if (File.Exists(ratingsPath) == false)
            {
                throw new Exception(string.Format("file {0} is not exists.", ratingsPath));
            }

            if (File.Exists(tagsPath) == false)
            {
                throw new Exception(string.Format("file {0} is not exists.", tagsPath));
            }

            using (var streamReader = new StreamReader(moviesPath))
            {
                builder.AddMovies(LoadMoviesFromFile(streamReader));
            }

            using (var streamReader = new StreamReader(ratingsPath))
            {
                builder.AddRatings(LoadRatingsFromFile(streamReader));
            }

            using (var streamReader = new StreamReader(tagsPath))
            {
                builder.AddTags(LoadTagsFromFile(streamReader));
            }

            return builder.Build();
        }

        private static IList<MovieRow> LoadMoviesFromFile(StreamReader stream)
        {
            List<MovieRow> list = new List<MovieRow>();
            string line;
            stream.ReadLine();
            while ((line = stream.ReadLine()) != null)
            {
                int comma1 = line.IndexOf(',');
                int comma2 = line.LastIndexOf(',');
                var id = int.Parse(line.Substring(0, comma1));
                var geners = line.Substring(comma2 + 1);
                var title = line.Substring(comma1 + 1, comma2 - comma1 - 1);
                MovieRow q = new MovieRow() {Id = id, Genres = geners, Title = title};
                list.Add(q);
            }

            return list;
        }

        private static IList<RatingRow> LoadRatingsFromFile(StreamReader stream)
        {
            List<RatingRow> list = new List<RatingRow>();
            string line;
            stream.ReadLine();
            while ((line = stream.ReadLine()) != null)
            {
                var parts = line.Split(',');
                var userId = int.Parse(parts[0]);
                var movieId = int.Parse(parts[1]);
                var raiting = float.Parse(parts[2].Replace(".", ","));
                var timeStamp = long.Parse(parts[3]);
                var q = new RatingRow() { UserId = userId, MovieId = movieId, RatingValue = raiting, Timestamp = timeStamp};
                list.Add(q);
            }

            return list;
        }

        private static IList<TagRow> LoadTagsFromFile(StreamReader stream)
        {
            List<TagRow> list = new List<TagRow>();
            string line;
            stream.ReadLine();
            while ((line = stream.ReadLine()) != null)
            {
                int comma1 = line.IndexOf(',');
                int comma2 = line.IndexOf(',', comma1 + 1);
                int comma3 = line.LastIndexOf(',');
                var userId = int.Parse(line.Substring(0, comma1));
                var movieId = int.Parse(line.Substring(comma1 + 1, comma2 - comma1 - 1));
                var tag = line.Substring(comma2 + 1, comma3 - comma2 - 1);
                var timestamp = long.Parse(line.Substring(comma3 + 1));
                var q = new TagRow() { UserId = userId, MovieId = movieId, Tag = tag, Timestamp = timestamp};
                list.Add(q);
            }

            return list;
        }

        private static void SaveToXml(MovieDataSet movieDataSet, string outputPath)
        {
            var genres = new List<MovieGenre>();
            var ratings = new List<MovieRating>();
            var tags = new List<MovieTag>();
            var users = new List<XmlModels.User>();
            var movies = new List<XmlModels.Movie>();

            foreach (var g in movieDataSet.Genres)
            {
                genres.Add(new MovieGenre() { Name = g.Name });
            }

            foreach (var u in movieDataSet.Users)
            {
                var xmlUser = new XmlModels.User() { Id = u.Id, Name = u.Name, RatedMovies = new List<int>(), TaggedMovies = new List<int>() };
                users.Add(xmlUser);
            }

            foreach (var m in movieDataSet.Movies)
            {
                var xmlMovie = new XmlModels.Movie() { Id = m.Id, Title = m.Title, Year = m.Year, Genres = new List<MovieGenre>(), Ratings = new List<MovieRating>(), Tags = new List<MovieTag>() };
                foreach (var g in m.Genres)
                {
                    xmlMovie.Genres.Add(genres.Find(x => x.Name == g.Name));
                }

                foreach (var t in m.Tags)
                {
                    var movieTag = new MovieTag() { Tag = t.Tag, TagDate = t.TagDate, UserId = t.User.Id };
                    users.Find(u => u.Id == t.User.Id).TaggedMovies.Add(xmlMovie.Id);
                    xmlMovie.Tags.Add(movieTag);
                }

                foreach (var r in m.Ratings)
                {
                    var movieRaiting = new MovieRating() { Rating = r.Rating, RateDate = r.RateDate, UserId = r.User.Id };
                    users.Find(u => u.Id == r.User.Id).RatedMovies.Add(xmlMovie.Id);
                }

                movies.Add(xmlMovie);
            }

            MovieDatabase movieDatabase = new MovieDatabase
            {
                Users = users,
                Movies = movies
            };
            using (var fileStream = new FileStream(outputPath, FileMode.Create))
            {
                // TODO Implement XML serialization using XmlModels classes and XmlSerializer. Use Example.xml as an example for XML structure.
                XmlSerializer serializer = new XmlSerializer(typeof(MovieDatabase));
                serializer.Serialize(fileStream, movieDatabase);
            }
        }
    }
}
