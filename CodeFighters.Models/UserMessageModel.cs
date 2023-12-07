using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighters.Models
{
    public class UserMessageModel : BaseModel
    {
        public virtual UserModel Sender { get; set; }
        public virtual UserModel Receiver { get; set; }
        public string MessageContent { get; set; }
        public bool IsRead { get; set; }
    }
}
