using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighters.Models
{
    public class ProfileAvatar : BaseModel
    {
        public string Head { get; set; }
        public string Body { get; set; }
        public string Accessory { get; set; }
    }
}
