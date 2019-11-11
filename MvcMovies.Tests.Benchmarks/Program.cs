using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MvcMovie.Controllers;
using MvcMovie.Models;

namespace MvcMovies.Tests.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SearchTests>();
        }
    }

    [HtmlExporter]
    public class SearchTests
    {
        [Params(10, 100)] public int NumberOfSearches;

        [Params(
            null, 
            "Romantic Comedy", 
            "Comedy", 
            "Western")] 
        public string GenreSearch;

        [Params(null,
            "When Harry Met Sally",
            "Ghostbusters",
            "Ghostbusters 2",
            "Rio Bravo")]
        public string TitleSearch;

        [Benchmark]
        public void PrintResultsToConsole()
        {
            var movieDbContext = new MovieDBContext();
            var controller = new MoviesController(movieDbContext);

            for (var i = 1; i <= NumberOfSearches; i++)
            {
                var result = controller.Index(GenreSearch, TitleSearch) as ViewResult;

                var model = result.Model as IEnumerable<Movie>;

                foreach (var movie in model)
                {
                    Console.WriteLine($"Result {movie.Title}");
                }
            }
        }

    }
}
