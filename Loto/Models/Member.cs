using Loto.Handle;
using System;
using System.Collections.Generic;

namespace Loto.Models
{
    public class Member
    {
        public string UserId { get; set; }
        public string  UserName { get; set; }
        public string RoomId { get; set; }
        public decimal Money { get; set; }
        public List<UserDefinedTableNumber> ListCheckingBingo { get; set; }
        public string TableHtml { get; set; }

        public Member()
        {
            List<UserDefinedTableNumber> tmp;

            TableHtml = NumberProcess.UserDefinedTableNumber(out tmp);

            ListCheckingBingo = new List<UserDefinedTableNumber>(tmp);
            this.Money = 100000;
            tmp.Clear();
        }

    }
}