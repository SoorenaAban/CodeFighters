using CodeFighters.GameMaster;
using Microsoft.AspNetCore.Mvc;
using CodeFighters.GameMaster;
using Microsoft.AspNetCore.Authentication;

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
