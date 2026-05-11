namespace MoveAPI.Services
{
    public interface IGenresServices
    {
        Task<IEnumerable<Genre>> GetAllAsync();

        Task<Genre> Add(Genre genre);

        Task<Genre> GetById(byte id);

        Task<Genre> Delete(Genre genre);

        Genre Update(Genre genre);
        
    }
}