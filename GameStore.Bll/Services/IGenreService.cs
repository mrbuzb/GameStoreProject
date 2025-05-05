using GameStore.Bll.Dto_s;

namespace GameStore.Bll.Services;

public interface IGenreService
{
    Task<Guid> AddGenreAsync(GenreCreateDto request);
    Task<List<GenreGetDto>> GetGenresByGameKeyAsync(string key);
    Task<GenreDto> GetGenreByIdAsync(Guid id);
    Task<List<GenreGetDto>> GetAllGenresAsync();
    Task<List<GenreGetDto>> GetGenresByParentIdAsync(Guid parentId);
    Task UpdateGenreAsync(GenreDto request);
    Task DeleteGenreAsync(Guid id);

}