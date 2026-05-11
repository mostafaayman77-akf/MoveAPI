using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MoveAPI.Dtos
{
    public class CreateMoviesDto
    {
        [MaxLength(250)]
        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }
        [MaxLength(2500)]
        public string StoryLine { get; set; }

       public IFormFile Poster { get; set; }

        public byte GenreId { get; set; }
    }
}
