using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighters.Models
{
    public class LogModel : BaseModel
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public int Type { get; set; }
    }
}
