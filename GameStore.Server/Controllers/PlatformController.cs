using GameStore.Bll.Dto_s;
using GameStore.Bll.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController (IPlatformService _platformService) : ControllerBase
    {
        [HttpPost("addPlatforms")]
        public async Task<Guid> PostPlatform(PlatformCreateDto platformCreateDto)
        {
            var platformDTO = await _platformService.AddPlatformAsync(platformCreateDto);
            return platformDTO;
        }
    }
}
