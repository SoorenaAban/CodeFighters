namespace CodeFighters.Models
{
    public class GameQuestion : BaseModel
    {
        public string Content { get; set; }


        public string Prompt { get; set; }
        public string RawResponse { get; set; }
        public DateTime GeneratedOn { get; set; }

        public Game Game { get; set; }
    }
}
