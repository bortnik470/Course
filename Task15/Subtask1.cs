using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Task15
{
    internal class Subtask1
    {
        public static void start()
        {
            int[] ints = { 1, 2, 3, 4, 5, 6, };
            List<string> stringList = new();
            stringList.Add("a");
            stringList.Add("5");
            stringList.Add("6");
            stringList.Add("3g");
            stringList.Add("10fox");
            stringList.Add("20apples");

            var result = ints.Select(i => stringList
                                          .FirstOrDefault(s => s.Length == i && char.IsNumber(s[0])) ?? "Not Found");
            

            foreach(var _string in result)
            {
                Console.WriteLine(_string);
            }
        }
    }
}
