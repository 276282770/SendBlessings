using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MessageModel
    {
        public string Content { get; set; }
        public string Nickname { get; set; }
        public string Avator { get; set; }
        public ObjectType ObjectType { get; set; }
        public int ObjectIndex { get; set; }
    }
}
