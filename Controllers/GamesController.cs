using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlinePreferance2_api.Database.Repositories;
using OnlinePreferance2_api.Model;
using OnlinePreferance2_api.Model.Auth;
using OnlinePreferance2_api.Services;

namespace OnlinePreferance2_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Game> _gameRepository;
        public GamesController(UserManager<ApplicationUser> userManager, IGenericRepository<Game> gameRepository)
        {
            _userManager = userManager;
            _gameRepository = gameRepository;
        }

        [HttpGet("GameVievItem")]
        public ActionResult<GameVievItem> GetGameVievItem(int? gameId = null)
        {
            IEnumerable<Game> games;
            if (!gameId.HasValue)
            {
                games = _gameRepository.GetAll();
            }
            else
            {
                games = _gameRepository.GetAll().Where(g => g.Id == gameId.Value);
            }

            return Ok(games.Select(x => x.ToPoco()));
        }
        [HttpGet()]
        public async Task<ActionResult<GameVievItem>> Get(int gameId)
        {
            var game = await _gameRepository.GetById(gameId);
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult> Post(GameCreateModel input)
        {
            var newGame = await GameService.CreateNewGame(input, _userManager);

            await _gameRepository.Create(newGame);
            return CreatedAtAction("Post", newGame);
        }
    }
}
