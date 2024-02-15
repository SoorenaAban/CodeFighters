using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighters.Models
{
    public class GameLogModel : BaseModel
    {
        public virtual GameModel Game { get; set; }
        public virtual UserModel User { get; set; }
        public string Log { get; set; }
    }
}
