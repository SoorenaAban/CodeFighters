using CodeFighters.WebApi.Models;

namespace CodeFighters.WebApi.Dto
{
    public class PlayerDto
    {
        public PlayerDto(User player)
        {
            Username = player.Username;
            DisplayName = player.DisplayName;
            ProfilePictureUrl = player.ProfilePictureUrl;
        }

        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}
