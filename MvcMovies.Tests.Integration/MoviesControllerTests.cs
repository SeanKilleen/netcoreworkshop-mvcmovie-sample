using System;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using MvcMovie.Controllers;
using MvcMovie.Models;
using Xunit;

namespace MvcMove.Tests.Integration
{
    public class MovieControllerTests : IDisposable
    {
        private readonly MoviesController _sut;
        private readonly MovieDBContext _dbContext;
        private readonly DbContextTransaction _transaction;

        public MovieControllerTests()
        {
            _dbContext = new MovieDBContext();
            _transaction = _dbContext.Database.BeginTransaction();

            _dbContext.Movies.RemoveRange(_dbContext.Movies.ToList());

            var aRomanceMovie = new Movie { ID = 1, Genre = "Romance", Price = 1.00m, Rating = "A+", ReleaseDate = DateTime.Now, Title = "A Romance Movie" };
            var romeoAndJuliet = new Movie { ID = 2, Genre = "Romance", Price = 1.00m, Rating = "A+", ReleaseDate = DateTime.Now, Title = "Romeo and Juliet" };
            var zoolander = new Movie { ID = 3, Genre = "Comedy", Price = 1.00m, Rating = "A+", ReleaseDate = DateTime.Now, Title = "Romeo and Juliet" };
            var aComedyMovie = new Movie { ID = 4, Genre = "Comedy", Price = 1.00m, Rating = "A+", ReleaseDate = DateTime.Now, Title = "A Comedy Movie" };

            _dbContext.Movies.Add(aRomanceMovie);
            _dbContext.Movies.Add(romeoAndJuliet);
            _dbContext.Movies.Add(zoolander);
            _dbContext.Movies.Add(aComedyMovie);

            _sut = new MoviesController(_dbContext);
        }

        public void Dispose()
        {
            _transaction.Rollback();
        }

        [Fact]
        public void DBStartsWithThreeMovies()
        {
            _dbContext.Movies.ToList().Count.Should().Be(4);
        }

        [Fact]
        public void AddingMovieMakes5Movies()
        {
            var newMov = new Movie
            {
                Price = 9.99m,
                Genre = "SciFi",
                Title = "Hello World",
                Rating = "PG",
                ReleaseDate = DateTime.Now
            };

            _dbContext.Movies.Add(newMov);

            _dbContext.SaveChanges();

            _dbContext.Movies.ToList().Count.Should().Be(5);
        }
    }
}