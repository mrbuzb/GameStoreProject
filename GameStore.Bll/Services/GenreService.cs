using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Bll.Dto_s;
using GameStore.Dal.Entities;
using GameStore.Repository.Services;

namespace GameStore.Bll.Services;

public class GenreService(IGenreRepository _genreRepo) : IGenreService
{
    public async Task<Guid> AddGenreAsync(GenreCreateDto request)
    {
        if(request.Name is null)
        {
            throw new ArgumentNullException("Genre name is required");
        }
        var genre = new Genre();
        genre.Name = request.Name;
        if(request.ParentGenreId != null)
        {
            try
            {
                await _genreRepo.GetGenreByIdAsync(request.ParentGenreId.Value);
            }
            catch(Exception ex)
            {
                throw new Exception("Parent Genre Not Found");
            }
            genre.ParentGenreId = request.ParentGenreId.Value;
        }

        Guid checkGenreExists;

        try
        {
            checkGenreExists = await _genreRepo.CheckGenreByNameAsync(request.Name);
        }
        catch (Exception ex)
        {
            await _genreRepo.AddAsync(genre);
            return genre.Id;
        }

        return checkGenreExists;
    }


    public async Task<List<GenreDto>> GetGenresByGameKeyAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGenreAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GenreDto>> GetAllGenresAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GenreDto> GetGenreByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GenreDto>> GetGenresByParentIdAsync(Guid parentId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateGenreAsync(GenreDto request)
    {
        throw new NotImplementedException();
    }
}
