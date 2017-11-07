using System;
using System.Collections.Generic;
using System.Linq;
using CsvParserApp.CsvModels;
using CsvParserApp.Models;

namespace CsvParserApp
{
    /// <summary>
    /// Represents a builder for <see cref="IMovieDataSetBuilder"/>. Read more about Builder design pattern: https://refactoring.guru/design-patterns/builder
    /// </summary>
    public class MovieDataSetBuilder : IMovieDataSetBuilder
    {
        private List<MovieRow> Movies { get; set; }
        private List<RatingRow> Ratings { get; set; }
        private List<TagRow> Tags { get; set; }

        public void AddMovies(IEnumerable<MovieRow> movies)
        {
            if (movies == null)
            {
                throw new ArgumentNullException(nameof(movies));
            }

            Movies = movies.ToList();
        }

        public void AddRatings(IEnumerable<RatingRow> ratings)
        {
            if (ratings == null)
            {
                throw new ArgumentNullException(nameof(ratings));
            }

            Ratings = ratings.ToList();
        }

        public void AddTags(IEnumerable<TagRow> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            Tags = tags.ToList();
        }

        public MovieDataSet Build()
        {
            var movieDataSet = new MovieDataSet();

            foreach (var movieRow in Movies)
            {
                var nameYear = movieRow.Title.Trim('"').TrimEnd(' ');
                var name = nameYear.Substring(0, nameYear.LastIndexOf(' '));
                var year = int.Parse(nameYear.Substring(nameYear.LastIndexOf(' ') + 1).TrimStart('(').TrimEnd(')').Substring(0, 4));
                var movie = new Movie() {Id = movieRow.Id, Title = name, Year = year};

                var genreNames = movieRow.Genres.Split('|');
                foreach (var genreName in genreNames)
                {
                    var dataSetGenre = movieDataSet.Genres.Find(g => g.Name == genreName);
                    if (dataSetGenre == null)
                    {
                        movieDataSet.Genres.Add(new Genre() {Name = genreName});
                        dataSetGenre = movieDataSet.Genres.Find(g => g.Name == genreName);
                    }
                    dataSetGenre.Movies.Add(movie);
                    movie.Genres.Add(dataSetGenre);
                }

                movieDataSet.Movies.Add(movie);
            }

            foreach (var tagRow in Tags)
            {
                DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(tagRow.Timestamp / 1000d)).ToLocalTime();
                var dataSetMovie = movieDataSet.Movies.Find(m => m.Id == tagRow.MovieId);
                var dataSetUser = movieDataSet.Users.Find(u => u.Id == tagRow.UserId);
                if (dataSetUser == null)
                {
                    movieDataSet.Users.Add(new User() {Id = tagRow.UserId, Name = "User" + tagRow.UserId });
                    dataSetUser = movieDataSet.Users.Find(u => u.Id == tagRow.UserId);
                }

                var userTag = new UserTag() {Movie = dataSetMovie, Tag = tagRow.Tag, TagDate = date, User = dataSetUser};
                dataSetUser.Tags.Add(userTag);
                dataSetMovie.Tags.Add(userTag);
            }

            foreach (var ratingRow in Ratings)
            {
                DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(ratingRow.Timestamp / 1000d)).ToLocalTime();
                var dataSetMovie = movieDataSet.Movies.Find(m => m.Id == ratingRow.MovieId);
                var dataSetUser = movieDataSet.Users.Find(u => u.Id == ratingRow.UserId);
                if (dataSetUser == null)
                {
                    movieDataSet.Users.Add(new User() { Id = ratingRow.UserId, Name = "User" + ratingRow.UserId });
                    dataSetUser = movieDataSet.Users.Find(u => u.Id == ratingRow.UserId);
                }

                var userRating = new UserRating() { Movie = dataSetMovie, Rating = ratingRow.RatingValue, RateDate = date, User = dataSetUser };
                dataSetUser.Ratings.Add(userRating);
                dataSetMovie.Ratings.Add(userRating);
            }

            return movieDataSet;
        }
    }
}