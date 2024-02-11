namespace CodeFighters.Models
{
    public class UserModel : BaseModel
    {

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public ProfileAvatar Avatar { get; set; }
        public uint Score { get; set; }

        public int BattleScore { get; set; }

        public virtual ICollection<GameModel> Games { get; set; }
        public virtual ICollection<ReportModel> Reports { get; set; }
        public virtual ICollection<ReportModel> ReportsMade { get; set; }
        public virtual ICollection<UserMessageModel> MessagesSent { get; set; }
        public virtual ICollection<UserMessageModel> MessagesReceived { get; set; }

    }
}
