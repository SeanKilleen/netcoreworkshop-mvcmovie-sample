using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MvcMovie.Controllers;
using MvcMovie.Models;

namespace MvcMovies.Tests.Benchmarks
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<SearchTests>();
        }
    }

    [HtmlExporter]
    public class SearchTests
    {
        private readonly int NumberOfSearches = 100;

        [Benchmark]
        public void WriteAllMovies()
        {
            var movieDbContext = new MovieDBContext();
            var controller = new MoviesController(movieDbContext);

            for (var i = 1; i <= NumberOfSearches; i++)
            {
                var result = controller.Index(movieGenre:null, searchString:null) as ViewResult;

                var model = result.Model as IEnumerable<Movie>;

                foreach (var movie in model)
                {
                    Console.WriteLine($"Result {movie.Title}");
                }
            }
        }

        [Benchmark]
        public void SearchByGenre()
        {
            var movieDbContext = new MovieDBContext();
            var controller = new MoviesController(movieDbContext);

            var movieGenre = "Comedy";
            for (var i = 1; i <= NumberOfSearches; i++)
            {
                var result = controller.Index(movieGenre: movieGenre, searchString: null) as ViewResult;

                var model = result.Model as IEnumerable<Movie>;

                foreach (var movie in model)
                {
                    Console.WriteLine($"Result {movie.Title}");
                }
            }
        }
        [Benchmark]
        public void SearchByTitle()
        {
            var movieDbContext = new MovieDBContext();
            var controller = new MoviesController(movieDbContext);

            var movieTitle = "Ghostbusters";
            for (var i = 1; i <= NumberOfSearches; i++)
            {
                var result = controller.Index(movieGenre: null, searchString: movieTitle) as ViewResult;

                var model = result.Model as IEnumerable<Movie>;

                foreach (var movie in model)
                {
                    Console.WriteLine($"Result {movie.Title}");
                }
            }
        }
    }
}
