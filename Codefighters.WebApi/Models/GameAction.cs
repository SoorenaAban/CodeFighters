namespace CodeFighters.WebApi.Models
{
    public class GameAction : BaseModel
    {
        public Game Game { get; set; }
        public User User { get; set; }

    }
}
