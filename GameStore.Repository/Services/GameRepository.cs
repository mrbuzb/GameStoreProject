using GameStore.Dal;
using GameStore.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repository.Services;

public class GameRepository(MainContext _mainContext) : IGameRepository
{
    public async Task<Guid> AddGameAsync(Game game)
    {
        await _mainContext.Games.AddAsync(game);
        await _mainContext.SaveChangesAsync();
        return game.Id;
    }

    public async Task DeleteGameAsync(Guid Id)
    {
        var game = await GetGameByIdAsync(Id);
        _mainContext.Games.Remove(game);
        await _mainContext.SaveChangesAsync();
    }

    private async Task LoadSubGenresAsync(Genre genre)
    {
        if (genre == null)
        {
            return;
        }

        var subGenres = await _mainContext.Genres
            .Where(g => g.ParentGenreId == genre.Id)
            .ToListAsync();

        genre.SubGenres = subGenres;

        foreach (var sub in subGenres)
        {
            await LoadSubGenresAsync(sub);
        }
    }

    public async Task<List<Game>> GetAllGamesAsync()
    {
        var games = await _mainContext.Games
            .Include(x => x.GamePlatforms)
            .Include(g => g.GameGenres)
            .ToListAsync();

        foreach (var game in games)
        {
            if (game.GameGenres != null)
            {
                foreach (var genre in game.GameGenres)
                {
                    await LoadSubGenresAsync(genre.Genre);
                }
            }
        }

        return games;
    }


    public async Task<Game> GetGameByIdAsync(Guid id)
    {
        var game = await _mainContext.Games.Include(g => g.GamePlatforms).Include(c => c.GameGenres).FirstOrDefaultAsync(x => x.Id == id);
        if (game == null)
        {
            throw new Exception();
        }

        if (game.GameGenres != null)
        {
            foreach (var genre in game.GameGenres)
            {
                if (game.GameGenres != null)
                    await LoadSubGenresAsync(genre.Genre);
            }
        }
        return game;
    }

    public async Task<Game> GetGameByKeyAsync(string key)
    {
        var game = await _mainContext.Games.Include(g => g.GamePlatforms).ThenInclude(pp=>pp.Platform).Include(c => c.GameGenres).ThenInclude(gg=>gg.Genre).FirstOrDefaultAsync(x => x.Key == key);
        if (game == null)
        {
            throw new Exception();
        }

        if (game.GameGenres != null)
        {
            foreach (var genre in game.GameGenres)
            {
                await LoadSubGenresAsync(genre.Genre);
            }
        }
        return game;
    }

    public async Task<List<Game>> GetGamesByGenreIdAsync(Guid genreId)
    {
        var games = await _mainContext.GameGenres
                .Where(gg => gg.GenreId == genreId)
                .Include(gg => gg.Game)
                .ThenInclude(g => g.GamePlatforms)
                .Include(gg => gg.Game)
                .ThenInclude(g => g.GameGenres)
                .Select(gg => gg.Game)
                .ToListAsync();

        foreach (var game in games)
        {
            if (game.GameGenres != null)
            {
                foreach (var genre in game.GameGenres)
                {
                    await LoadSubGenresAsync(genre.Genre);
                }
            }
        }
        return games;
    }



    public async Task<List<Game>> GetGamesByPlatformIdAsync(Guid platformId)
    {
        var games = await _mainContext.GamePlatforms
                .Where(gp => gp.PlatformId == platformId)
                .Include(gp => gp.Game)
                .ThenInclude(g => g.GamePlatforms)
                .Include(gp => gp.Game)
                .ThenInclude(g => g.GameGenres)
                .Select(gp => gp.Game)
                .ToListAsync();


        foreach (var game in games)
        {
            if (game.GameGenres != null)
            {
                foreach (var genre in game.GameGenres)
                {
                    await LoadSubGenresAsync(genre.Genre);
                }
            }
        }
        return games;
    }


    public async Task UpdateGameAsync(Game game)
    {
        _mainContext.Games.Update(game);
        await _mainContext.SaveChangesAsync();
    }
}
