﻿using CodeFighters.Data;
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


        [Authorize]
        [HttpPost]
        [Route("new/{targetUsername}")]
        public IActionResult New(string targetUsername)
        {
            var currentUsername = HttpContext.User.Identity.Name;
            var currentUser = _apiContext.Users.FirstOrDefault(u => u.Username == currentUsername);
            bool isVsAI = false;
            if (targetUsername == "server")
                isVsAI = true;
            if (currentUser == null) 
            {
                return NotFound();
            }

            var game = new GameModel();

            game.Players = new List<UserModel>
            {
                currentUser
            };
            if (!isVsAI)
            {
                var targetUser = _apiContext.Users.FirstOrDefault(u => u.Username == targetUsername);
                if (targetUser == null)
                {
                    return NotFound();
                }
                game.Players.Add(targetUser);
            }
            //game.IsVsAI = isVsAI;
            game.PlayerOneId = currentUser.Id;
            game.PlayerOneReady = false;
            game.PlayerTwoReady = false;
            //if(!isVsAI)
            //    game.PlayerTwoReady = false;
            //else game.PlayerTwoReady = true;
            game.PlayerOneAnswered = false;
            game.PlayerTwoAnswered = false;
            game.IsActive = true;
            game.IsRunning = false;
            game.HasStarted = false;
            game.PlayerOneHealth = 10;
            game.PlayerTwoHealth = 10;
            game.QuestionCount = 10;
            game.TurnNumber = 1;
            game.CreatedAt = DateTime.Now;
            game.Result = 1;

            _apiContext.Games.Add(game);
            _apiContext.SaveChanges();
            _gameMaster.CreateGame(game, _apiContext);
            _gameMaster.StartGame(game.Id);
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

    }
}
