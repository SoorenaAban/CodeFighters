using CodeFighters.Data;
using CodeFighters.WebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeFighters.Models;

namespace CodeFighters.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly ApiContext _apiContext;

        public ReportController(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] ReportPostDto reportPostDto)
        {
            var reportingUsername = User.Identity.Name;
            var reportingUser = _apiContext.Users.Find(reportingUsername);

            var reportedUser = _apiContext.Users.Find(reportPostDto.ReportedUsername);

            if (reportedUser == null)
            {
                return NotFound();
            }

            var report = new ReportModel
            {
                ReportingUser = reportingUser,
                ReportedUser = reportedUser,
                Reason = reportPostDto.Report
            };

            _apiContext.Reports.Add(report);
            _apiContext.SaveChanges();
            return Ok();
        }
    }
}
