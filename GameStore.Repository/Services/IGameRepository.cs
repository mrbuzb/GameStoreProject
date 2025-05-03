using GameStore.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GameStore.Repository.Services;

public interface IGameRepository
{
    Task<Game> GetGameByKeyAsync(string key);
    Task<Game> GetGameByIdAsync(Guid id);
    Task<List<Game>> GetAllGamesAsync();
    Task<List<Game>> GetGamesByGenreIdAsync(Guid genreId);
    Task<List<Game>> GetGamesByPlatformIdAsync(Guid platformId);
    Task<Guid> AddGameAsync(Game game);
    Task UpdateGameAsync(Game game);
    Task DeleteGameAsync(Guid Id);

}