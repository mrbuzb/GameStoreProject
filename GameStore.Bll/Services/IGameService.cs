using GameStore.Bll.Dto_s;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Bll.Services;

public interface IGameService
{
    Task<Guid> AddGameAsync(GameCreateDto request);
    Task<GameDto> GetGameByKeyAsync(string key);
    Task<GameDto> GetGameByIdAsync(Guid id);
    Task<List<GameDto>> GetAllGamesAsync();
    Task<List<GameDto>> GetGamesByGenreAsync(Guid genreId);
    Task<List<GameDto>> GetGamesByPlatformAsync(Guid platformId);
    Task UpdateGameAsync(UpdateGameDto request);
    Task DeleteGameAsync(Guid Id);
    Task<byte[]> GetGameFileAsync(Guid id);
}