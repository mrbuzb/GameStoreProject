using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Bll.Dto_s;
using GameStore.Dal.Entities;
using GameStore.Repository.Services;

namespace GameStore.Bll.Services;

public class PlatformService(IPlatformRepository _platformRepo) : IPlatformService
{
    public async Task<Guid> AddPlatformAsync(PlatformCreateDto request)
    {
        if(request.Type is null)
        {
            throw new ArgumentNullException("Platform type is required");
        }
        var platform = new Platform();
        platform.Type = request.Type;
        Guid id;
        try
        {
            id =await _platformRepo.CheckPlatformType(request.Type);
        }
        catch (Exception ex)
        {
            await _platformRepo.AddPlatformAsync(platform);
            return platform.Id;
        }
        return id;
    }

    public Task DeletePlatformAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PlatformDto>> GetAllPlatformsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<PlatformDto> GetPlatformByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<PlatformDto>> GetPlatformsByGameKeyAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePlatformAsync(PlatformDto request)
    {
        throw new NotImplementedException();
    }
}
