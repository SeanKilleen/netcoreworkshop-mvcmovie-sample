using MvcMovie.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using Moq;
using MvcMovie.Models;
using Xunit;
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable LocalizableElement
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleMultipleEnumeration

namespace MvcMovie.Tests.Unit
{
    public class MoviesControllerTests
    {
        public class Ctor
        {
            [Fact]
            public void WithNullDbContext_ThrowsException()
            {
                Action act = () => new MoviesController(null);

                act.Should().Throw<ArgumentNullException>()
                    .And.ParamName.Should().Be("dbContext");
            }

            [Fact]
            public void WithNonNullDbContext_DoesNotThrow()
            {
                var mock = new Mock<IMovieDbContext>();

                Action act = () => new MoviesController(mock.Object);

                act.Should().NotThrow();
            }
        }

        public class Index
        {
            private readonly MoviesController _sut;
            private readonly IQueryable<Movie> _allMovies;

            private readonly Movie _anActionMovie = new Movie { ID = 1, Genre = "Action", Title = "An Action movie" };
            private readonly Movie _speed = new Movie { ID = 1, Genre = "Action", Title = "Speed" };
            private readonly Movie _aRomanceMovie = new Movie { ID = 1, Genre = "Romance", Title = "A Romance movie" };
            private readonly Movie _romeoAndJuliet = new Movie { ID = 1, Genre = "Romance", Title = "Romeo and Juliet" };
            private readonly Movie _aComedyMovie = new Movie { ID = 1, Genre = "Comedy", Title = "A Comedy movie" };
            private readonly Movie _zoolander = new Movie { ID = 1, Genre = "Comedy", Title = "Zoolander" };

            public Index()
            {
                var mockDb = new Mock<IMovieDbContext>();

                _allMovies = new List<Movie>
                {
                    _anActionMovie,
                    _speed,
                    _aRomanceMovie,
                    _romeoAndJuliet,
                    _aComedyMovie,
                    _zoolander
                }.AsQueryable();

                var mockSet = new Mock<DbSet<Movie>>();
                mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(_allMovies.Provider);
                mockSet.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(_allMovies.Expression);
                mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(_allMovies.ElementType);
                mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(_allMovies.GetEnumerator());

                mockDb.Setup(x => x.Movies).Returns(mockSet.Object);

                _sut = new MoviesController(mockDb.Object);
            }

            [Fact]
            public void SetsViewBagMovieGenreToDistinctListOfGenres()
            {
                var result = _sut.Index("", "") as ViewResult;
                var x = result.ViewBag.MovieGenre as SelectList;

                x.Count().Should().Be(3);
                x.Should().Contain(thing => thing.Text == "Romance");
                x.Should().Contain(thing => thing.Text == "Comedy");
                x.Should().Contain(thing => thing.Text == "Action");
            }

            [Fact]
            public void ReturnsFullListOfMoviesIfNoSearchStrings()
            {
                var result = _sut.Index("", "") as ViewResult;

                var model = result.ViewData.Model as IEnumerable<Movie>;

                model.Count().Should().Be(6);
                model.Should().BeEquivalentTo(_allMovies);
            }

            [Fact]
            public void IfGenreProvided_FiltersByGenre()
            {
                var expectedResults = new List<Movie>
                {
                    _aRomanceMovie,
                    _romeoAndJuliet
                };

                var result = _sut.Index("Romance", "") as ViewResult;
                var model = result.ViewData.Model as IEnumerable<Movie>;

                model.Count().Should().Be(2);
                model.Should().BeEquivalentTo(expectedResults);
            }

            [Fact]
            public void GenreFilterIsExactMatch()
            {
                var partialGenreString = "Rom";

                var result = _sut.Index(partialGenreString, "") as ViewResult;
                var model = result.ViewData.Model as IEnumerable<Movie>;

                model.Count().Should().Be(0);
            }

            [Fact]
            public void GenreSearchIsCaseSensitive()
            {
                var lowerCaseGenre = "romance";

                var result = _sut.Index(lowerCaseGenre, "") as ViewResult;
                var model = result.ViewData.Model as IEnumerable<Movie>;

                model.Count().Should().Be(0);
            }

            [Fact]
            public void WhenTitleSearchProvided_FiltersByTitle()
            {
                var expectedResults = new List<Movie>
                {
                    _aRomanceMovie
                };

                var result = _sut.Index("", "Romance") as ViewResult;
                var model = result.ViewData.Model as IEnumerable<Movie>;

                model.Should().BeEquivalentTo(expectedResults);
            }

            [Fact]
            public void TitleSearchIsContains()
            {
                var expectedResults = new List<Movie>
                {
                    _aRomanceMovie,
                    _romeoAndJuliet
                };

                var result = _sut.Index("", "Rom") as ViewResult;
                var model = result.ViewData.Model as IEnumerable<Movie>;

                model.Should().BeEquivalentTo(expectedResults);
            }

            [Fact]
            public void TitleSearchIsCaseSensitive()
            {
                var result = _sut.Index("", "rom") as ViewResult;
                var model = result.ViewData.Model as IEnumerable<Movie>;

                model.Count().Should().Be(0);
            }

            [Fact]
            public void WhenBothSearchesProvided_FiltersByBoth()
            {
                var expectedResults = new List<Movie>
                {
                    _romeoAndJuliet
                };

                var result = _sut.Index("Romance", "Jul") as ViewResult;
                var model = result.ViewData.Model as IEnumerable<Movie>;

                model.Should().BeEquivalentTo(expectedResults);
            }
        }

        public class IndexWithFormCollection
        {
            private readonly MoviesController _sut;

            public IndexWithFormCollection()
            {
                var mockDb = new Mock<IMovieDbContext>();
                _sut = new MoviesController(mockDb.Object);
            }

            [Fact]
            public void ReturnsStringWithSearchTerm()
            {
                var searchString = "blahblah";

                var result = _sut.Index(new FormCollection(), searchString);

                result.Should().Be($"<h3> From [HttpPost]Index: {searchString}</h3>");
            }
        }

        public class Details
        {
            private readonly MoviesController _sut;

            public Details()
            {
                var mockDb = new Mock<IMovieDbContext>();
                _sut = new MoviesController(mockDb.Object);

                mockDb = new Mock<IMovieDbContext>();

                var allMovies = new List<Movie>
                {
                    new Movie{ID = 1, Title = "MovieOne"},
                    new Movie{ID = 2, Title = "MovieTwo"}
                }.AsQueryable();

                var mockSet = new Mock<DbSet<Movie>>();
                mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(allMovies.Provider);
                mockSet.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(allMovies.Expression);
                mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(allMovies.ElementType);
                mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(allMovies.GetEnumerator());

                mockSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(findParamArray => allMovies.ToList().FirstOrDefault(y => y.ID == (int)findParamArray[0]));

                mockDb.Setup(x => x.Movies).Returns(mockSet.Object);

                _sut = new MoviesController(mockDb.Object);
            }

            [Fact]
            public void NoID_ReturnsBadRequest()
            {
                var result = _sut.Details(null) as HttpStatusCodeResult;

                result.StatusCode.Should().Be(400);
            }

            [Fact]
            public void WhenMovieNotFound_ReturnNotFoundResult()
            {
                var result = _sut.Details(3);

                result.GetType().Name.Should().Be("HttpNotFoundResult");
            }

            [Fact]
            public void WhenMovieFound_ReturnViewWithMovie()
            {
                var result = _sut.Details(2) as ViewResult;

                var model = result.ViewData.Model as Movie;

                model.Title.Should().Be("MovieTwo");
            }
        }
    }
}
