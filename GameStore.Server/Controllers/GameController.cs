using GameStore.Bll.Dto_s;
using GameStore.Bll.Services;
using GameStore.Repository.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IGameRepository _gameRepo;

        public GameController(IGameService gameService, IGameRepository gameRepo)
        {
            _gameService = gameService;
            _gameRepo = gameRepo;
        }

        [HttpPost("addGames")]
        public async Task<Guid> PostGame(GameCreateDto gameCreateDto)
        {
            var gameDTO = await _gameService.AddGameAsync(gameCreateDto);
            Response.StatusCode = StatusCodes.Status201Created;
            return gameDTO;
        }

        [HttpPut("update-game")]
        public async Task UpdateGame(UpdateGameDto game)
        {
            await _gameService.UpdateGameAsync(game);
        }
        [HttpDelete("dellete-game")]
        public async Task DeleteGame(Guid id)
        {
            await _gameService.DeleteGameAsync(id);
        }


        [HttpGet("get-games")]
        public async Task<List<GameDto>> GetAllGamesAsync()
        {
            return await _gameService.GetAllGamesAsync();
        }



        [HttpGet("download-game/{id}")]
        public async Task<IActionResult> DownloadGame(Guid id)
        {
            try
            {
                var fileBytes = await _gameService.GetGameFileAsync(id);
                var game = await _gameRepo.GetGameByIdAsync(id);
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = $"{game.Name}_{timestamp}.txt";

                return File(fileBytes, "text/plain", fileName);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("get-gameByKey")]
        public async Task<GameDto> GetGameByKey(string key)
        {
            return await _gameService.GetGameByKeyAsync(key);
        }
        [HttpGet("get-gameById")]
        public async Task<GameDto> GetGameById(Guid id)
        {
            return await _gameService.GetGameByIdAsync(id);
        }
        [HttpGet("get-gamesByGenre")]
        public async Task<List<GameDto>> GetGamesByGenre(Guid genreId)
        {
            return await _gameService.GetGamesByGenreAsync(genreId);
        }
        [HttpGet("get-gamesByPlatform")]
        public async Task<List<GameDto>> GetGamesByPlatform(Guid platformId)
        {
            return await _gameService.GetGamesByPlatformAsync(platformId);
        }
        
    }
}
