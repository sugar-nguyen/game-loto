using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loto.Handle
{
    public static class NumberProcess
    {
        public static int[] CreateArrayGameNumber()
        {
            return Enumerable.Range(1, 99).OrderBy(c => Guid.NewGuid()).ToArray();
        }

        public static string UserDefinedTableNumber(out List<UserDefinedTableNumber> outObj)
        {
            var array = CreateArrayGameNumber();
            int[] arr1 = array.Where(x => x <= 9).ToArray();
            int[] arr2 = array.Where(x => x >= 10 && x < 20).OrderBy(c => Guid.NewGuid()).ToArray();
            int[] arr3 = array.Where(x => x >= 20 && x < 30).OrderBy(c => Guid.NewGuid()).ToArray();
            int[] arr4 = array.Where(x => x >= 30 && x < 40).OrderBy(c => Guid.NewGuid()).ToArray();
            int[] arr5 = array.Where(x => x >= 40 && x < 50).OrderBy(c => Guid.NewGuid()).ToArray();
            int[] arr6 = array.Where(x => x >= 50 && x < 60).OrderBy(c => Guid.NewGuid()).ToArray();
            int[] arr7 = array.Where(x => x >= 60 && x < 70).OrderBy(c => Guid.NewGuid()).ToArray();
            int[] arr8 = array.Where(x => x >= 70 && x < 80).OrderBy(c => Guid.NewGuid()).ToArray();
            int[] arr9 = array.Where(x => x >= 80 && x < 90).OrderBy(c => Guid.NewGuid()).ToArray();
            int[] arr10 = array.Where(x => x >= 90 && x < 99).OrderBy(c => Guid.NewGuid()).ToArray();

            var listArr = new List<int[]>();/* { arr1, arr2, arr3, arr4, arr5, arr6, arr7, arr8, arr9, arr10 };*/

            for (int i = 0; i < 5; i++)
            {
                listArr.Add(new int[10] { arr1[i], arr2[i], arr3[i], arr4[i], arr5[i], arr6[i], arr7[i], arr8[i], arr9[i], arr10[i] });
            }

            List<UserDefinedTableNumber> userNumberCheckingLst = new List<UserDefinedTableNumber>();
            string html = "";
            for (int i = 0; i < 5; i++)
            {
                var arrMakeHole = MakeHole(listArr[i]);
                html += "<tr>";
                var tmpObj = new UserDefinedTableNumber();
                foreach (var y in listArr[i])
                {
                   
                    if (Array.Exists(arrMakeHole, x => x == y))
                    {
                        html += "<td></td>";
                    }
                    else
                    {
                        html += "<td><a href='#' data-type='" + y + "' class='td-pick'>" + y + "</a></td>";
                        tmpObj.ls.Add(new PickNumber() { Number = y});
                    }
                }
                html += "</tr>";
                userNumberCheckingLst.Add(tmpObj);
            }
            outObj = new List<UserDefinedTableNumber>(userNumberCheckingLst);
            return html;
        }

        private static int[] MakeHole(int[] arr)
        {
            if (arr.Length == 0)
            {
                return null;
            }

            var _arr = arr;
            List<int> arrResult = new List<int>();

            Random rd = new Random();
            while (arrResult.Count < 5)
            {
                int y = rd.Next(0, _arr.Length);
                arrResult.Add(_arr[y]);
                _arr = Array.FindAll(_arr, x => x != _arr[y]);
            }

            return arrResult.ToArray();
        }
    }
    public class UserDefinedTableNumber
    {
        public List<PickNumber> ls { get; set; }

        public UserDefinedTableNumber()
        {
            ls = new List<PickNumber>();
        }

    }

    public class PickNumber
    {
        public int Number { get; set; }
        public bool Pick { get; set; }
        public PickNumber()
        {
            Pick = false;
        }
    }
}
