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

        public virtual ICollection<GameModel> Games { get; set; }
    }
}
