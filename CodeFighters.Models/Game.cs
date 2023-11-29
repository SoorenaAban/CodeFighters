namespace CodeFighters.Models
{
    public class Game : BaseModel
    {

        public Game(User startedBy, User targetUser)
        {
            PlayerOne = startedBy;
            PlayerOneHealth = 100;
            PlayerTwo = targetUser;
            PlayerTwoHealth = 100;
            Turn = targetUser;
            TurnNumber = 1;
            QuestionNumber = 1;
            QuestionCount = 10;
            StartTime = DateTime.Now;
            IsActive = true;

        }

        public User PlayerOne { get; set; }
        public int PlayerOneHealth { get; set; }
        public User PlayerTwo { get; set; }
        public int PlayerTwoHealth { get; set; }
        public int TurnNumber { get; set; }
        public int QuestionNumber { get; set; }
        public int QuestionCount { get; set; }
        public User Turn { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
        public GameQuestion CurrentQuestion { get; set; }

        public ICollection<GameQuestion> Questions { get; set; }

    }
}
