using CodeFighters.GameMaster;
using Microsoft.AspNetCore.Mvc;
using CodeFighters.GameMaster;

namespace CodeFighters.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameMasterController : Controller
    {
        private readonly IGameMaster _gameMaster;

        public GameMasterController(IGameMaster gameMaster)
        {
            _gameMaster = gameMaster;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return _gameMaster.GameCode != null ? Ok(_gameMaster.GameCode) : NotFound();
        }

        [HttpPost]
        [Consumes("text/plain")]
        public IActionResult SubmitGameCode([FromBody]string code)
        {
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
