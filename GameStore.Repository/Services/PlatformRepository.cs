using GameStore.Dal;
using GameStore.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repository.Services;

public class PlatformRepository(MainContext _mainContext) : IPlatformRepository
{
    public async Task<Guid> AddPlatformAsync(Platform platform)
    {
        await _mainContext.Platforms.AddAsync(platform);
        await _mainContext.SaveChangesAsync();
        return platform.Id;
    }

    public async Task<bool> CheckPlatformIdAsync(Guid id)
    {
        return await _mainContext.Platforms.AnyAsync(p => p.Id == id);
    }

    public async Task<Guid> CheckPlatformType(string type)
    {
        var platform =await _mainContext.Platforms.FirstOrDefaultAsync(p => p.Type == type);
        if (platform is null)
        {
            throw new Exception();
        }
        return platform.Id;
    }

    public async Task DeletePlatformAsync(Guid id)
    {
        var platform = await GetPlatformByIdAsync(id);
        _mainContext.Platforms.Remove(platform);
        await _mainContext.SaveChangesAsync();
    }

    public async Task<List<Platform>> GetAllPlatformAsync()
    {
        return await _mainContext.Platforms.ToListAsync();
    }

    public async Task<Platform> GetPlatformByIdAsync(Guid id)
    {
        var platform = await _mainContext.Platforms.FirstOrDefaultAsync(x => x.Id == id);
        if (platform == null)
        {
            throw new Exception("Platform not found.");
        }
        return platform;
    }

    public async Task<List<Platform>> GetPlatformsByGameKeyAsync(string key)
    {
        var game = await _mainContext.Games.FirstOrDefaultAsync(game => game.Key == key);
        if (game is null)
        {
            throw new Exception();
        }
        var platformsOfGame = new List<Platform>();
        foreach (var _ in game.GamePlatforms)
        {
            platformsOfGame.Add(_.Platform);
        }
        return platformsOfGame;
    }

    public async Task UpdatePlatformAsync(Platform platform)
    {
        _mainContext.Platforms.Update(platform);
        await _mainContext.SaveChangesAsync();
    }
}
