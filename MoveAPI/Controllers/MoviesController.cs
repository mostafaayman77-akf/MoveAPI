using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoveAPI.Dtos;
using MoveAPI.Models;

namespace MoveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly new List<string> allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private long maxAllowedPosterSize = 4194304;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _context.Movies.OrderByDescending(x => x.Rate).Include(m => m.Genre).Select(m => new MovieDetailDto
            {
                Id = m.Id,
                GenreId = m.GenreId,
                GenreName = m.Genre.Name,
                Poster = m.Poster,
                Rate = m.Rate,
                StoryLine = m.StoryLine,
                Title = m.Title,
                Year = m.Year

            }).ToListAsync();
            return Ok(movies);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound();
            var dto = new MovieDetailDto
            {
                Id = movie.Id,
                GenreId = movie.GenreId,
                GenreName = movie.Genre?.Name,
                Poster = movie.Poster,
                Rate = movie.Rate,
                StoryLine = movie.StoryLine,
                Title = movie.Title,
                Year = movie.Year
            };
            return Ok(dto);
        }
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(int genreId)
        {
            var movies = await _context.Movies.Where(m => m.GenreId == genreId).OrderByDescending(x => x.Rate).Include(m => m.Genre).Select(m => new MovieDetailDto
            {
                Id = m.Id,
                GenreId = m.GenreId,
                GenreName = m.Genre.Name,
                Poster = m.Poster,
                Rate = m.Rate,
                StoryLine = m.StoryLine,
                Title = m.Title,
                Year = m.Year
            }).ToListAsync();
            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateMoviesDto dto)
        {
            if(dto.Poster == null)
                return BadRequest("Poster is required");

            if (!allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg, .jpeg, .png images are allowed");

            if (dto.Poster.Length > maxAllowedPosterSize)
                return BadRequest("Poster size must be less than 4MB");

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid GenreId");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Rate = dto.Rate,
                StoryLine = dto.StoryLine,
                Poster = dataStream.ToArray(),
                GenreId = dto.GenreId
            };
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return Ok(movie);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] CreateMoviesDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No Movie found with the given ID {id}");

            if (!allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg, .jpeg, .png images are allowed");

            if (dto.Poster.Length > maxAllowedPosterSize)
                return BadRequest("Poster size must be less than 4MB");

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid GenreId");
            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;
            movie.StoryLine = dto.StoryLine;
            movie.Poster = dataStream.ToArray();
            movie.GenreId = dto.GenreId;
             _context.SaveChanges();
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No Movie found with the given ID {id}");
            _context.Movies.Remove(movie);
             _context.SaveChanges();

            return Ok(movie);

        }
    }
}