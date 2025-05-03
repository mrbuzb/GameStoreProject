using GameStore.Bll.Dto_s;

namespace GameStore.Bll.Services;

public interface IGenreService
{
    Task<Guid> AddGenreAsync(GenreCreateDto request);
    Task<List<GenreDto>> GetGenresByGameKeyAsync(string key);
    Task<GenreDto> GetGenreByIdAsync(Guid id);
    Task<IEnumerable<GenreDto>> GetAllGenresAsync();
    Task<IEnumerable<GenreDto>> GetGenresByParentIdAsync(Guid parentId);
    Task UpdateGenreAsync(GenreDto request);
    Task DeleteGenreAsync(Guid id);

}