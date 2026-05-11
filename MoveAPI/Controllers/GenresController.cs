using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoveAPI.Dtos;
using MoveAPI.Models;
using MoveAPI.Services;

namespace MoveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresServices _genresServices;

        public GenresController(IGenresServices genresServices)
        {
            _genresServices = genresServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres=await _genresServices.GetAllAsync();
            return Ok(genres);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAllAsync(CreateGenreDto dto) 
        {
            var genre=new Genre { Name = dto.Name };
            await _genresServices .Add(genre);
            return Ok(genre);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateAsync(int id, [FromBody]CreateGenreDto dto) 
        {
            var genre=await _genresServices.GetById((byte)id);
            if (genre == null) return NotFound($"No genre was found with ID:{id}");
            genre.Name=dto.Name;
            _genresServices.Update(genre);
            return Ok(genre);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteAsync(int id) 
        {
            var genre=await _genresServices.GetById((byte)id);
            if (genre == null) return NotFound($"No genre was found with ID:{id}");
            _genresServices.Delete(genre);
            return Ok(genre);
        }
};
}
