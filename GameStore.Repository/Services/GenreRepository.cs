using GameStore.Dal;
using GameStore.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repository.Services;

public class GenreRepository(MainContext _mainContext) : IGenreRepository
{

    public async Task AddAsync(Genre genre)
    {
        await _mainContext.Genres.AddAsync(genre);
        await _mainContext.SaveChangesAsync();
    }

    public async Task<Guid> CheckGenreByNameAsync(string name)
    {
        var genre =await _mainContext.Genres.FirstOrDefaultAsync(x =>x.Name == name);
        if(genre == null)
        {
            throw new Exception();
        }
        return genre.Id;
    } 

    public async Task DeleteAsync(Guid id)
    {
        var genre = await _mainContext.Genres.FindAsync(id);
        if (genre != null)
        {
            _mainContext.Genres.Remove(genre);
            await _mainContext.SaveChangesAsync();
        }
        throw new Exception();
    }

    public async Task<bool> CheckGenreIdAsync(Guid id)
    {
        return await _mainContext.Genres.AnyAsync(gen => gen.Id == id);
    }

    public async Task<List<Genre>> GetAllGenreAsync()
    {
        return await _mainContext.Genres.ToListAsync();
    }

    public async Task<Genre> GetGenreByIdAsync(Guid id)
    {
        var genre =  await _mainContext.Genres.FindAsync(id);
        if(genre != null)
        {
            throw new Exception();
        }
        return genre!;
    }

    public async Task<List<Genre>> GetGenreByParentIdAsync(Guid parentId)
    {
        return await _mainContext.Genres
                             .Where(g => g.ParentGenreId == parentId)
                             .ToListAsync();
    }

    public async Task UpdateAsync(Genre genre)
    {
        _mainContext.Genres.Update(genre);
        await _mainContext.SaveChangesAsync();
    }

    public Task<List<Genre>> GetGenresByGameKeyAsync(string key)
    {
        throw new NotImplementedException();
    }
}
