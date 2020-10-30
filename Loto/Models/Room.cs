using Loto.Handle;
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
        public int CountOfTimePlay { get; set; }
        public int[] Shuffule { get; set; }
        public Room()
        {
            Members = new List<Member>();
            Shuffule = NumberProcess.CreateArrayGameNumber();
            CountOfTimePlay = 1;
        }
    }
}
