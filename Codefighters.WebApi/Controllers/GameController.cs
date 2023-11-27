using CodeFighters.WebApi.Data;
using CodeFighters.WebApi.Dto;
using CodeFighters.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CodeFighters.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly ApiContext _apiContext;

        public GameController(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var games = _apiContext.Games.Any() ? _apiContext.Games : null;
            var gameDtos = new List<GameOverviewDto>();
            foreach (var game in games)
            {
                gameDtos.Add(new GameOverviewDto(game));
            }

            return Ok(gameDtos);
        }

        [Authorize]
        [HttpGet]
        [Route("active")]
        public IActionResult GetActive()
        {
            var activeGames = _apiContext.Games.Where(g => g.IsActive);
            var activeGameDtos = new List<GameOverviewDto>();
            foreach (var game in activeGames)
            {
                activeGameDtos.Add(new GameOverviewDto(game));
            }

            return Ok(activeGameDtos);
        }

        [Authorize]
        [HttpPost]
        [Route("start/{target}")]
        public IActionResult Start(string target)
        {
            var targetUser = _apiContext.Users.FirstOrDefault(u => u.Username == target);
            if (targetUser == null)
                return NotFound();

            var currentUsername = HttpContext.User.Identity.Name;
            var currentUser = _apiContext.Users.FirstOrDefault(u => u.Username == currentUsername);
            if(currentUser == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var game = new Game(currentUser , targetUser);
            _apiContext.Games.Add(game);
            _apiContext.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("{gameId:guid}/action/{action}")]
        public IActionResult Action(Guid gameId, string action)
        {
            var game = _apiContext.Games.FirstOrDefault(g => g.Id == gameId);
            if (game == null)
                return NotFound();

            var currentUsername = HttpContext.User.Identity.Name;
            var currentUser = _apiContext.Users.FirstOrDefault(u => u.Username == currentUsername);
            if(currentUser == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            if(game.Turn.Id != currentUser.Id)
                return StatusCode(StatusCodes.Status403Forbidden);

            

            _apiContext.SaveChanges();
            return Ok();
        }   
    }
}
