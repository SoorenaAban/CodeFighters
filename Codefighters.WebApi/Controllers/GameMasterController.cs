using CodeFighters.GameMaster;
using Microsoft.AspNetCore.Mvc;
using CodeFighters.Data;
using CodeFighters.GameMaster;

using Microsoft.AspNetCore.Authentication;
using CodeFighters.WebApi.Dto;

namespace CodeFighters.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameMasterController(IGameMaster gameMaster, ApiContext apiContext) : Controller
    {
        private readonly IGameMaster _gameMaster = gameMaster;
        private readonly ApiContext _apiContext = apiContext;

        [HttpGet]
        public IActionResult Index()
        {
            var gameCodes = _apiContext.GameCodes.OrderBy(gc => gc.IsValid).ToList();
            var gameCodeDtos = new List<CodeDto>();
            foreach (var gameCode in gameCodes)
                gameCodeDtos.Add(new CodeDto(gameCode));
            return Ok(gameCodeDtos);
        }

        [HttpPost]
        [Consumes("text/plain")]
        public IActionResult SubmitGameCode([FromBody]string code)
        {
            string validationToken = "435662";
            //check if token is in code
            if (!code.Contains(validationToken))
                return BadRequest("validation token not present");

            _gameMaster.GameCode = code;
            var validator = new GameCodeValidator(code);
            if (!validator.Validate())
            {
                return BadRequest(validator.ErrorMessage);
            }
            return Ok();
        }
    }
}
