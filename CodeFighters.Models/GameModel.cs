namespace CodeFighters.Models
{
    public class GameModel : BaseModel
    {
        public const int MAX_SEC_WAITING_FOR_PLAYERS = 30;

        public GameModel()
        {
            PlayerOneHealth = 100;
            PlayerTwoHealth = 100;
            TurnNumber = 1;
            QuestionNumber = 1;
            QuestionCount = 10;
            StartTime = DateTime.Now;
            IsActive = true;
            IsRunning = false;
        }
        public GameModel(UserModel startedBy, UserModel targetUser)
        {
            Players = new List<UserModel>
            {
                startedBy,
                targetUser
            };
            PlayerOneHealth = 100;
            PlayerTwoHealth = 100;
            Turn = targetUser;
            TurnNumber = 1;
            QuestionNumber = 1;
            QuestionCount = 10;
            StartTime = DateTime.Now;
            IsActive = true;
            IsRunning = false;

        }

        public virtual ICollection<UserModel> Players { get; set; }

        public UserModel PlayerOne => Players.First();
        public int PlayerOneHealth { get; set; }
        public bool PlayerOneReady { get; set; }
        public UserModel PlayerTwo => Players.Last();
        public int PlayerTwoHealth { get; set; }
        public bool PlayerTwoReady { get; set; }
        public int TurnNumber { get; set; }
        public int QuestionNumber { get; set; }
        public int QuestionCount { get; set; }
        public UserModel Turn { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool HasStarted { get; set; }
        public bool IsActive { get; set; }
        public bool IsRunning { get; set; }
        //public GameQuestionModel CurrentQuestion { get; set; }

        public virtual ICollection<GameQuestionModel> Questions { get; set; }

    }
}
