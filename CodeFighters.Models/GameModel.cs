using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFighters.Models
{
    public class GameModel : BaseModel
    {
        public const int MAX_SEC_WAITING_FOR_PLAYERS = 60;

        public bool IsVsAI { get; set; }
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
        [NotMapped]
        public bool PlayerOneAnswered { get; set; }
        public int PlayerTwoHealth { get; set; }
        public bool PlayerTwoReady { get; set; }
        [NotMapped]
        public bool PlayerTwoAnswered { get; set; }
        public int TurnNumber { get; set; }
        public int QuestionCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool HasStarted { get; set; }
        public bool IsActive { get; set; }
        public bool IsRunning { get; set; }
        /// <summary>
        /// 1- not over
        /// 2- player one won
        /// 3- player two won
        /// 4- tie
        /// </summary>
        public int Result { get; set; }

        public virtual ICollection<GameQuestionModel> Questions { get; set; }

    }
}
