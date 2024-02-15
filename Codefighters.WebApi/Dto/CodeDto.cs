using CodeFighters.Models;

namespace CodeFighters.WebApi.Dto
{
    public class CodeDto
    {
        public CodeDto(GameCodeModel gameCode)
        {
            Code = gameCode.Code;
            IsActive = gameCode.IsActive;
            IsValid = gameCode.IsValid;
            ErrorMessage = gameCode.ErrorMessage;
            CreatedDate = gameCode.CreatedAt;
        }

        public string Code { get; set; }
        public bool IsActive { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
