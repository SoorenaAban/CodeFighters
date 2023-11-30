using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighters.Models
{
    public class ReportModel : BaseModel
    {
        public UserModel ReportingUser { get; set; }
        public UserModel ReportedUser { get; set; }
        public string Reason { get; set; }
    }
}
