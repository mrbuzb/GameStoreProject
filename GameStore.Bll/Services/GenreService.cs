using AutoMapper;
using GameStore.Bll.Dto_s;
using GameStore.Dal.Entities;
using GameStore.Repository.Services;

namespace GameStore.Bll.Services;

public class GenreService(IGenreRepository _genreRepo, IMapper _mapper) : IGenreService
{
    public async Task<Guid> AddGenreAsync(GenreCreateDto request)
    {
        if (request.Name is null)
        {
            throw new ArgumentNullException("Genre name is required");
        }
        var genre = new Genre();
        genre.Name = request.Name;
        if (request.ParentGenreId != null)
        {
            try
            {
                await _genreRepo.GetGenreByIdAsync(request.ParentGenreId.Value);
            }
            catch (Exception ex)
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


    public async Task<List<GenreGetDto>> GetGenresByGameKeyAsync(string key)
    {
        var genres = await _genreRepo.GetGenresByGameKeyAsync(key);
        return genres.Select(x => _mapper.Map<GenreGetDto>(x)).ToList();
    }

    public async Task DeleteGenreAsync(Guid id)
    {
        await _genreRepo.DeleteAsync(id);
    }

    public async Task<List<GenreGetDto>> GetAllGenresAsync()
    {
        var genres = await _genreRepo.GetAllGenreAsync();
        return genres.Select(x => _mapper.Map<GenreGetDto>(x)).ToList();
    }

    public async Task<GenreDto> GetGenreByIdAsync(Guid id)
    {
        var genre = await _genreRepo.GetGenreByIdAsync(id);
        return _mapper.Map<GenreDto>(genre);
    }

    public async Task<List<GenreGetDto>> GetGenresByParentIdAsync(Guid parentId)
    {
        var genres = await _genreRepo.GetGenreByParentIdAsync(parentId);
        return genres.Select(x => _mapper.Map<GenreGetDto>(x)).ToList();
    }

    public async Task UpdateGenreAsync(GenreDto request)
    {
        var oldGenre = await _genreRepo.GetGenreByIdAsync(request.Id);

        if (request.ParentGenreId is not null)
        {
            if (await _genreRepo.GetGenreByIdAsync(request.ParentGenreId!.Value) is null)
            {
                throw new Exception();
            }
        }
        oldGenre.ParentGenreId = request.ParentGenreId;
        oldGenre.Name = request.Name;
        await _genreRepo.UpdateAsync(oldGenre);
    }
}
