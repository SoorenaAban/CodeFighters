namespace CodeFighters.WebApi.Models
{
    public class User : BaseModel
    {

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string ProfilePictureUrl { get; set; }

        public ICollection<Game> Games { get; set; }
        public ICollection<Game> GamesStarted { get; set; }
        public ICollection<Game> TurnsIn { get; set; }
    }
}
