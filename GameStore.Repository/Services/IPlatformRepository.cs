using GameStore.Dal.Entities;

namespace GameStore.Repository.Services;

public interface IPlatformRepository
{
    Task<Platform> GetPlatformByIdAsync(Guid id);
    Task<List<Platform>> GetAllPlatformAsync();
    Task<Guid> AddPlatformAsync(Platform platform);
    Task<bool> CheckPlatformIdAsync(Guid id);
    Task UpdatePlatformAsync(Platform platform);
    Task<Guid> CheckPlatformType(string type);
    Task<List<Platform>> GetPlatformsByGameKeyAsync(string key);
    Task DeletePlatformAsync(Guid Id);
}