using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using GameStore.Bll.Dto_s;
using GameStore.Dal;
using GameStore.Dal.Entities;
using GameStore.Repository.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace GameStore.Bll.Services;

public class GameService(IGameRepository _repoGame, IPlatformRepository _repoPlatform, IGenreRepository _repoGenre, MainContext _context, IMapper _mapper,IMemoryCache _cache) : IGameService
{
    private const string _cacheKey = "games_list";
    public async Task<Guid> AddGameAsync(GameCreateDto request)
    {
        //Validation
        request.PlatformIds = FixDuplications(request.PlatformIds);
        request.GenreIds = FixDuplications(request.GenreIds);

        foreach (var genreId in request.GenreIds)
        {
            if (!await _repoGenre.CheckGenreIdAsync(genreId))
                throw new Exception("Some genres do not exist.");
        }

        foreach (var platformId in request.PlatformIds)
        {
            if (!await _repoPlatform.CheckPlatformIdAsync(platformId))
                throw new Exception("Some platforms do not exist.");
        }
        //Validation
        var gameEntity = new Game
        {
            Name = request.Name,
            Description = request.Description,
        };

        if (string.IsNullOrEmpty(request.Key) || request.Key.Length < 8)
        {
            var clearKey = GenerateGameKey(request.Name);
            var shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8);
            clearKey = clearKey + "-" + shortGuid;
            gameEntity.Key = clearKey;
        }
        else
        {
            var checker = false;
            var key = GenerateGameKey(request.Key);
            try
            {
                await _repoGame.GetGameByKeyAsync(key);
            }
            catch (Exception ex)
            {
                gameEntity.Key = key;
                checker = true;
            }
            if (checker is false)
            {
                gameEntity.Key = key;
                var shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8);
                gameEntity.Key += "-" + shortGuid;
            }
        }


        await _repoGame.AddGameAsync(gameEntity);


        var gameGenreIds = new List<Guid>();
        var gamePlatformIds = new List<Guid>();

        foreach (var gameplt in gameEntity.GamePlatforms)
        {
            gamePlatformIds.Add(gameplt.PlatformId);
        }

        foreach (var gameggr in gameEntity.GameGenres)
        {
            gameGenreIds.Add(gameggr.GenreId);
        }


        foreach (var genreId in gameGenreIds)
        {
            var gameGenre = new GameGenre
            {
                GameId = gameEntity.Id,
                GenreId = genreId
            };
            await _context.GameGenres.AddAsync(gameGenre);
        }

        foreach (var platformId in gamePlatformIds)
        {
            var gamePlatform = new GamePlatform
            {
                GameId = gameEntity.Id,
                PlatformId = platformId
            };
            await _context.GamePlatforms.AddAsync(gamePlatform);
        }

        await _context.SaveChangesAsync();
        await RefreshMusicCacheAsync();
        return gameEntity.Id;
    }

    public async Task DeleteGameAsync(Guid Id)
    {
        await _repoGame.DeleteGameAsync(Id);
        await RefreshMusicCacheAsync();
    }

    public async Task<List<GameDto>> GetAllGamesAsync()
    {
        if (_cache.TryGetValue(_cacheKey, out List<Game> cachedGames))
        {
            return cachedGames!.Select(x=>_mapper.Map<GameDto>(x)).ToList();
        }


        var games = await _repoGame.GetAllGamesAsync();

        var gameDtos = games.Select(x => _mapper.Map<GameDto>(x)).ToList();

        _cache.Set(_cacheKey, games, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        });

        return gameDtos;
    }

    public async Task<GameDto> GetGameByIdAsync(Guid id)
    {
        if (_cache.TryGetValue(_cacheKey, out List<Game> cachedGames))
        {
            var gameDtos = cachedGames!.Select(x => _mapper.Map<GameDto>(x)).ToList();
            return gameDtos!.FirstOrDefault(_ => _.Id == id) ?? throw new ArgumentNullException();
        }
        var game = await _repoGame.GetGameByIdAsync(id);

        return _mapper.Map<GameDto>(game);
    }

    public async Task<GameDto> GetGameByKeyAsync(string key)
    {
        if (_cache.TryGetValue(_cacheKey, out List<Game> cachedGames))
        {
            var gameDtos = cachedGames!.Select(x => _mapper.Map<GameDto>(x)).ToList();
            return gameDtos!.FirstOrDefault(_ => _.Key == key) ?? throw new ArgumentNullException();
        }
        var game = await _repoGame.GetGameByKeyAsync(key);
        return _mapper.Map<GameDto>(game);
    }

    public async Task<byte[]> GetGameFileAsync(Guid id)
    {
        Game game;
        if (_cache.TryGetValue(_cacheKey, out List<Game> cachedGames))
        {
            game =  cachedGames!.FirstOrDefault(x => x.Id == id) ?? throw new ArgumentNullException("Game by id not found");
        }
        else
        {
            game = await _repoGame.GetGameByIdAsync(id);
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };

        var serialized = JsonSerializer.Serialize(game, options);

        using var memoryStream = new MemoryStream();
        using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
        {
            await writer.WriteAsync(serialized);
            await writer.FlushAsync();
        }

        return memoryStream.ToArray();
    }



    public async Task<List<GameDto>> GetGamesByGenreAsync(Guid genreId)
    {
        var games = await _repoGame.GetGamesByGenreIdAsync(genreId);
        return games.Select(_ => _mapper.Map<GameDto>(_)).ToList();
    }

    public async Task<List<GameDto>> GetGamesByPlatformAsync(Guid platformId)
    {
        var games = await _repoGame.GetGamesByPlatformIdAsync(platformId);
        return games.Select(_ => _mapper.Map<GameDto>(_)).ToList();
    }


    public async Task UpdateGameAsync(UpdateGameDto request)
    {
        //Validation
        if (request is null)
        {
            throw new Exception();
        }

        request.PlatformIds = FixDuplications(request.PlatformIds);
        request.GenreIds = FixDuplications(request.GenreIds);

        foreach (var genreId in request.GenreIds)
        {
            if (!await _repoGenre.CheckGenreIdAsync(genreId))
                throw new Exception("Some genres do not exist.");
        }

        foreach (var platformId in request.PlatformIds)
        {
            if (!await _repoPlatform.CheckPlatformIdAsync(platformId))
                throw new Exception("Some platforms do not exist.");
        }
        //Validation

        var gameEntity = await _repoGame.GetGameByIdAsync(request.Id);
        gameEntity.Id = request.Id;
        gameEntity.Name = request.Name;
        gameEntity.Description = request.Description;
        var checkKey = gameEntity.Key;

        var resultOfKey = await CheckGameKey(gameEntity, checkKey);

        if (string.IsNullOrEmpty(request.Key) || request.Key.Length < 8)
        {
            var clearKey = GenerateGameKey(request.Name);
            var shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8);
            clearKey = clearKey + "-" + shortGuid;
            gameEntity.Key = clearKey;
        }
        else
        {

            if (resultOfKey.Item1 == true)
            {
                gameEntity.Key = request.Key;
            }
            else
            {
                var key = GenerateGameKey(request.Key);
                gameEntity.Key = key;
                var shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8);
                gameEntity.Key += "-" + shortGuid;
            }
        }



        foreach (var genreId in request.GenreIds)
        {
            if (resultOfKey.Item2.GameGenres.FirstOrDefault(x => x.GenreId == genreId) is null)
            {
                var gameGenre = new GameGenre
                {
                    GameId = gameEntity.Id,
                    GenreId = genreId
                };
                await _context.GameGenres.AddAsync(gameGenre);
            }

        }

        foreach (var s in resultOfKey.Item2.GameGenres)
        {
            if (resultOfKey.Item2.GameGenres.FirstOrDefault(x => x.GenreId == s.GenreId) is null)
            {
                if (!request.GenreIds.Contains(s.GenreId))
                {
                    _context.GameGenres.Remove(s);
                }
            }
        }


        foreach (var platformId in request.PlatformIds)
        {
            if (resultOfKey.Item2.GamePlatforms.FirstOrDefault(x => x.PlatformId == platformId) is null)
            {
                var gamePlatform = new GamePlatform
                {
                    GameId = gameEntity.Id,
                    PlatformId = platformId
                };
                await _context.GamePlatforms.AddAsync(gamePlatform);
            }
        }

        foreach (var s in resultOfKey.Item2.GamePlatforms)
        {
            if (resultOfKey.Item2.GamePlatforms.FirstOrDefault(x => x.PlatformId == s.PlatformId) is null)
            {
                if (!request.PlatformIds.Contains(s.PlatformId))
                {
                    _context.GamePlatforms.Remove(s);
                }
            }
        }

        await _repoGame.UpdateGameAsync(gameEntity);
        await _context.SaveChangesAsync();
        await RefreshMusicCacheAsync();
    }


    private List<Guid> FixDuplications(List<Guid> ids)
    {
        var uniqueIds = new List<Guid>();
        foreach (var id in ids)
        {
            if (!uniqueIds.Contains(id))
            {
                uniqueIds.Add(id);
            }
        }
        return uniqueIds;
    }

    private string GenerateGameKey(string name)
    {
        var slug = name.ToLower()
                       .Replace(" ", "-")
                       .Replace(":", "")
                       .Replace(",", "")
                       .Replace(".", "")
                       .Replace("'", "")
                       .Replace("\"", "")
                       .Replace("&", "and");
        return slug;
    }


    private async Task<(bool, Game)> CheckGameKey(Game gameById, string gameKey)
    {
        Game gameByKey = new Game() { Key = "", };
        try
        {
            gameByKey = await _repoGame.GetGameByKeyAsync(gameKey);
        }
        catch (Exception ex)
        {
        }
        if (gameByKey.Id == gameById.Id)
        {
            return (true, gameById);
        }
        return (false, gameById);
    }

    private async Task RefreshMusicCacheAsync()
    {
        var gamesEntities = await _repoGame.GetAllGamesAsync();

        _cache.Set(_cacheKey, gamesEntities, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        });
    }


}
