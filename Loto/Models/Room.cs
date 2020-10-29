using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loto.Models
{
    public class Room
    {
        public string RoomId { get; set; }
        public List<Member> Members { get; set; }

        public Room()
        {
            Members = new List<Member>();
        }
    }
}
