using GameStore.Bll.Dto_s;
using GameStore.Bll.Services;
using GameStore.Dal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController (IPlatformService _platformService) : ControllerBase
    {
        [HttpPost("addPlatform")]
        public async Task<Guid> PostPlatform(PlatformCreateDto platformCreateDto)
        {
            var platformDTO = await _platformService.AddPlatformAsync(platformCreateDto);
            return platformDTO;
        }

        [HttpDelete("deletePlatform")]
        public async Task DeletePlatformAsync(Guid Id)
        {
            await _platformService.DeletePlatformAsync(Id);
        }

        [HttpPut("updatePlatform")]
        public async Task UpdatePlatformAsync(PlatformDto platform)
        {
            await _platformService.UpdatePlatformAsync(platform);
        }

        [HttpGet("getPlatformById")]
        public async Task<PlatformDto> GetPlatformByIdAsync(Guid id)
        {
            return await _platformService.GetPlatformByIdAsync(id);
        }
        [HttpGet("getAllPlatforms")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<List<PlatformDto>> GetAllPlatformsAsync()
        {
            return await _platformService.GetAllPlatformsAsync();
        }
        [HttpGet("getPlatformsByGameKey")]
        public async Task<List<PlatformDto>> GetPlatformsByGameKeyAsync(string key)
        {
            return await _platformService.GetPlatformsByGameKeyAsync(key);
        }
        
    }
}
