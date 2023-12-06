using System.ComponentModel.DataAnnotations;

namespace CodeFighters.WebApi.Dto
{
    public class ContactMessagePostDto
    {
        public required string Name { get; set; }
        public required string EmailAddress { get; set; }
        public required string MessageContent { get; set; }
    }
}
