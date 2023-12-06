namespace CodeFighters.WebApi.Dto
{
    public class ReportPostDto
    {
        public required string ReportedUsername { get; set; }
        public required string Report { get; set; }
    }
}
