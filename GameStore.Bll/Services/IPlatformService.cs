using GameStore.Bll.Dto_s;

namespace GameStore.Bll.Services;

public interface IPlatformService
{
    Task<Guid> AddPlatformAsync(PlatformCreateDto request);
    Task<PlatformDto> GetPlatformByIdAsync(Guid id);
    Task<List<PlatformDto>> GetAllPlatformsAsync();
    Task UpdatePlatformAsync(PlatformDto request);
    Task<List<PlatformDto>> GetPlatformsByGameKeyAsync(string key);
    Task DeletePlatformAsync(Guid id);

}