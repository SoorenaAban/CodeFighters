using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighters.Models
{
    public class GameCodeModel : BaseModel
    {
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }

        public virtual ICollection<GameModel> Games { get; set; }
    }
}
