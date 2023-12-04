using CodeFighters.Data;
using CodeFighters.WebApi.Dto;
using CodeFighters.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Text;
using CodeFighters.GameMaster;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace CodeFighters.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly ApiContext _apiContext;
        private readonly IGameMaster _gameMaster;

        public GameController(ApiContext apiContext, GameMaster.IGameMaster gameMaster)
        {
            _apiContext = apiContext;
            _gameMaster = gameMaster;
        }

        private static async Task Echo(WebSocket webSocket, GameModel gameModel)
        {


            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);



            buffer = Encoding.UTF8.GetBytes("Game Started.");
            await webSocket.SendAsync(
                new ArraySegment<byte>(buffer, 0, buffer.Length),
                receiveResult.MessageType,
                receiveResult.EndOfMessage,
                CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {


                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }

        [Authorize]
        [HttpPost]
        [Route("new/{targetUsername}")]
        public IActionResult New(string targetUsername)
        {
            var currentUsername = HttpContext.User.Identity.Name;
            var currentUser = _apiContext.Users.FirstOrDefault(u => u.Username == currentUsername);
            var targetUser = _apiContext.Users.FirstOrDefault(u => u.Username == targetUsername);
            if (currentUser == null || targetUser == null) 
            {
                return NotFound();
            }

            var game = new GameModel();
            game.Players = new List<UserModel>
            {
                currentUser,
                targetUser
            };
            game.PlayerOneId = currentUser.Id;
            game.PlayerOneReady = false;
            game.PlayerTwoReady = false;
            game.Turn = currentUser;
            game.IsActive = true;
            game.IsRunning = false;
            game.HasStarted = false;
            game.PlayerOneHealth = 100;
            game.PlayerTwoHealth = 100;
            game.QuestionCount = 10;
            game.QuestionNumber = 1;
            game.TurnNumber = 1;

            _apiContext.Games.Add(game);
            _apiContext.SaveChanges();
            _gameMaster.CreateGame(game, _apiContext);
            return Ok(game.Id);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var games = _apiContext.Games.Include(g => g.Players);
            var gameDtos = new List<GameOverviewDto>();
            foreach (var game in games)
            {
                gameDtos.Add(new GameOverviewDto(game));
            }

            return Ok(gameDtos);
        }

        private void PushMessages(string someMessageSource, WebSocket socket)
        {
            throw new NotImplementedException();
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

        //[Authorize]
        //[HttpPost]
        //[Route("ready/{target}")]
        //public IActionResult Ready(string target)
        //{
        //    var targetUser = _apiContext.Users.FirstOrDefault(u => u.Username == target);
        //    if (targetUser == null)
        //        return NotFound();

        //    var currentUsername = HttpContext.User.Identity.Name;
        //    var currentUser = _apiContext.Users.FirstOrDefault(u => u.Username == currentUsername);
        //    if(currentUser == null)
        //        return StatusCode(StatusCodes.Status500InternalServerError);

        //    var game = new GameModel(currentUser , targetUser);
        //    _apiContext.Games.Add(game);
        //    _apiContext.SaveChanges();
        //    return Ok();
        //}

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
