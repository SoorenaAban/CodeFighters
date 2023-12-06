using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeFighters.WebApi.Dto;
using CodeFighters.Data;
using CodeFighters.Models;

namespace CodeFighters.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ApiContext _context;

        public ContactController(ApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] ContactMessagePostDto contactMessageModel)
        {
            var contactMessage = new ContactMessageModel
            {
                Name = contactMessageModel.Name,
                EmailAddress = contactMessageModel.EmailAddress,
                Message = contactMessageModel.MessageContent
            };

            _context.ContactMessages.Add(contactMessage);
            _context.SaveChanges();

            return Ok();
        }
    }
}
