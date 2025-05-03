using GameStore.Bll.Dto_s;
using GameStore.Bll.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController(IGenreService _genreService) : ControllerBase
    {
        [HttpPost("addGenres")]
        public async Task<Guid> PostGenre(GenreCreateDto genreCreateDto)
        {
            var genreDTO = await _genreService.AddGenreAsync(genreCreateDto);
            return genreDTO;
        }
    }
}
