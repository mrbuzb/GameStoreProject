using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Bll.Dto_s;
using GameStore.Dal.Entities;
using GameStore.Repository.Services;

namespace GameStore.Bll.Services;

public class PlatformService(IPlatformRepository _platformRepo,IMapper _mapper) : IPlatformService
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

    public async Task DeletePlatformAsync(Guid id)
    {
        await _platformRepo.DeletePlatformAsync(id);
    }

    public async Task<List<PlatformDto>> GetAllPlatformsAsync()
    {
        var platforms = await _platformRepo.GetAllPlatformAsync();
        return platforms.Select(x=>_mapper.Map<PlatformDto>(x)).ToList();
    }

    public async Task<PlatformDto> GetPlatformByIdAsync(Guid id)
    {
        var platform = await _platformRepo.GetPlatformByIdAsync(id);
        return _mapper.Map<PlatformDto>(platform);
    }

    public async Task<List<PlatformDto>> GetPlatformsByGameKeyAsync(string key)
    {
        var platforms = await _platformRepo.GetPlatformsByGameKeyAsync(key);
        return platforms.Select(x=>_mapper.Map<PlatformDto>(x)).ToList();
    }

    public async Task UpdatePlatformAsync(PlatformDto request)
    {
        var platform = await _platformRepo.GetPlatformByIdAsync(request.Id);
        platform.Type = request.Type;
        await _platformRepo.UpdatePlatformAsync(platform);
    }


}
