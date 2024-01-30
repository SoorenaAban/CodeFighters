using CodeFighters.Models;

namespace CodeFighters.WebApi.Dto
{
    public class PlayerDto
    {
        public PlayerDto(UserModel player)
        {
            Username = player.Username;
            DisplayName = player.DisplayName;
            Avatar = new PlayerAvatarDto(player.Avatar);
        }

        public string Username { get; set; }
        public string DisplayName { get; set; }
        public PlayerAvatarDto Avatar { get; set; }
    }
}
