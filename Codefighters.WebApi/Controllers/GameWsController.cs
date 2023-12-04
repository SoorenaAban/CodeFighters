using CodeFighters.Data;
using CodeFighters.GameMaster;
using CodeFighters.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.WebSockets;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CodeFighters.WebApi.Controllers
{
    [ApiController]
    [Route("ws/game")]
    public class GameWsController : ControllerBase
    {

        private readonly ApiContext _apiContext;
        private readonly IGameMaster _gameMaster;

        public GameWsController(ApiContext apiContext, IGameMaster gameMaster)
        {
            _apiContext = apiContext;
            _gameMaster = gameMaster;
        }

        private async Task ReceiveMessages(WebSocket socket, GameModel gameModel)
        {
            var user = _apiContext.Users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (user == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }
            var isPlayerOne = gameModel.PlayerOneId == user.Id;
            var gameWorker = _gameMaster.GetGameWorker(gameModel.Id);

            var buffer = new byte[1024 * 4];
            var receiveResult = await socket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (socket.State == WebSocketState.Open)
            {
                receiveResult = await socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

                if (receiveResult.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    string playerAction = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);

                    gameWorker.UserInput(playerAction, isPlayerOne);
                }
            }
        }

        private async Task SendMessages(WebSocket socket, GameModel gameModel)
        {
            var gameWorker = _gameMaster.GetGameWorker(gameModel.Id);
            var user = _apiContext.Users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (user == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }

            bool isPlayerOne = gameModel.PlayerOneId == user.Id;

            while (socket.State == WebSocketState.Open)
            {
                string message = "";
                if (isPlayerOne)
                {
                    message = gameWorker.PlayerOneReturnMessage;
                }
                else
                {
                    message = gameWorker.PlayerTwoReturnMessage;
                }

                if (message != "")
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                       await Task.Delay(1000);
                }
            }
        }


        [Route("/ws/game/{gameid:guid}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize]
        public async Task GetGameWebSocet(Guid gameid)
        {
            var gameModel = _apiContext.Games.Where(g => g.Id == gameid).Include("Players").FirstOrDefault(g => g.Id == gameid);

            if (gameModel == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            var game = _gameMaster.GetGameWorker(gameid);
            if (game == null)
            {
                if (gameModel.IsActive)
                {
                    _gameMaster.CreateGame(gameModel, _apiContext);
                    game = _gameMaster.GetGameWorker(gameid);
                }
                else
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return;
                }
            }

            var user = _apiContext.Users.FirstOrDefault(u => u.Username == HttpContext.User.Identity.Name);
            if (user == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                return;
            }

            var checkPlayer = gameModel.Players.FirstOrDefault(p => p.Id == user.Id);

            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using (var socket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                    await Task.WhenAll(ReceiveMessages(socket, gameModel), SendMessages(socket, gameModel));
            }

            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
