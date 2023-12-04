using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFighters.Models
{
    public class GameModel : BaseModel
    {
        public const int MAX_SEC_WAITING_FOR_PLAYERS = 30;


        public virtual ICollection<UserModel> Players { get; set; }
        public Guid PlayerOneId { get; set; }

        [NotMapped]
        public UserModel PlayerOne
        {
            get
            {
                return Players.FirstOrDefault(p => p.Id == PlayerOneId);
            }
        }

        [NotMapped]
        public UserModel PlayerTwo
        {
            get
            {
                return Players.FirstOrDefault(p => p.Id != PlayerOneId);
            }
        }

        public int PlayerOneHealth { get; set; }
        public bool PlayerOneReady { get; set; }
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
