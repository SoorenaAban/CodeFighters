using CodeFighters.WebApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodeFighters.WebApi.Models;
using CodeFighters.WebApi.Data;

namespace CodeFighters.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ApiContext _context;   

        public UserController(ApiContext apiContext)
        {
            _context = apiContext;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok(_context.Users);
        }

        [HttpGet("{username}")]
        public ActionResult Get(string username)
        {
            return Ok();
        }
    }
}
