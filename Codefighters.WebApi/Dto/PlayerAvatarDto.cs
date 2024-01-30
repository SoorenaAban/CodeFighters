using CodeFighters.Models;

namespace CodeFighters.WebApi.Dto
{
    public class PlayerAvatarDto
    {
        public PlayerAvatarDto(ProfileAvatar avatar)
        {
            Head = avatar.Head;
            Body = avatar.Body;
            Accessory = avatar.Accessory;
        }

        public string Head { get; set; }
        public string Body { get; set; }
        public string Accessory { get; set; }
    }
}
