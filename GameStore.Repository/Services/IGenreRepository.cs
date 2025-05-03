using GameStore.Dal.Entities;

namespace GameStore.Repository.Services;

public interface IGenreRepository
{
    Task<Genre> GetGenreByIdAsync(Guid id);
    Task<List<Genre>> GetAllGenreAsync();
    Task<List<Genre>> GetGenreByParentIdAsync(Guid parentId);
    Task<List<Genre>> GetGenresByGameKeyAsync(string key);
    Task<bool> CheckGenreIdAsync(Guid id);
    Task<Guid> CheckGenreByNameAsync(string name);
    Task AddAsync(Genre genre);
    Task UpdateAsync(Genre genre);
    Task DeleteAsync(Guid Id);
}