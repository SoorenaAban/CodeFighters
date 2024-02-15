using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighters.Models
{
    public class GameErrorModel : BaseModel
    {
        public string ErrorMessage { get; set; }
        public virtual GameModel Game { get; set; }
        public virtual UserModel User { get; set; }
        public virtual GameCodeModel GameCode { get; set; }
    }
}
