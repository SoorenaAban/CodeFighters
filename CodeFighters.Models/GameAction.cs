namespace CodeFighters.Models
{
    public class GameAction : BaseModel
    {
        public GameModel Game { get; set; }
        public UserModel User { get; set; }

    }
}
