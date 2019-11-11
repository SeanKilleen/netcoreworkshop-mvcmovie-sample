using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MvcMovie.Models
{
    public class Movie
    {
        public int ID { get; set; }




        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }




        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }




        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }




        [Range(1, 100)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }




        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(5)]
        public string Rating { get; set; }
    }

    public interface IMovieDbContext : IDisposable
    {
        DbSet<Movie> Movies { get; set; }
        void SaveChanges();
        DbEntityEntry<Movie> Entry(Movie movie);
    }

    public class MovieDBContext : DbContext, IMovieDbContext
    {
        public DbSet<Movie> Movies { get; set; }

        void IMovieDbContext.SaveChanges()
        {
            this.SaveChanges();
        }

        DbEntityEntry<Movie> IMovieDbContext.Entry(Movie movie)
        {
            return this.Entry(movie);
        }
    }
}