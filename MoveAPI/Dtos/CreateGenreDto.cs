using System.ComponentModel.DataAnnotations;

namespace MoveAPI.Dtos
{
    public class CreateGenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
