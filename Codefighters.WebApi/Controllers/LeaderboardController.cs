using CodeFighters.Data;
using CodeFighters.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CodeFighters.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : Controller
    {
        private readonly ApiContext _context;

        public LeaderboardController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var leaderboard = new LeaderboardDto();
            leaderboard.Player = new List<PlayerDto>();
            var players = _context.Users.OrderByDescending(u => u.Score).Take(10).ToList();
            foreach (var player in players)
            {
                leaderboard.Player.Add(new PlayerDto(player));
            }
            return Ok(leaderboard);
        }
    }
}
