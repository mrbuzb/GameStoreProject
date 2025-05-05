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
        [HttpPut("updateGenre")]
        public async Task UpdateGenreAsync(GenreDto request)
        {
             await _genreService.UpdateGenreAsync(request);
        }
        [HttpDelete("deleteGenreAsync")]
        public async Task DeleteGenreAsync(Guid id)
        {
             await _genreService.DeleteGenreAsync(id);
        }
        [HttpGet("getGenresByGameKey")]
        public async Task<List<GenreGetDto>> GetGenresByGameKeyAsync(string key)
        {
            return await _genreService.GetGenresByGameKeyAsync(key);
        }
        [HttpGet("getGenreById")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<GenreDto> GetGenreByIdAsync(Guid id)
        {
            return await _genreService.GetGenreByIdAsync(id);
        }
        [HttpGet("getAllGenres")]
        public async Task<List<GenreGetDto>> GetAllGenresAsync()
        {
            return await _genreService.GetAllGenresAsync();
        }
        [HttpGet("getGenresByParentId")]
        public async Task<List<GenreGetDto>> GetGenresByParentIdAsync(Guid parentId)
        {
            return await _genreService.GetGenresByParentIdAsync(parentId);
        }
    }
}
