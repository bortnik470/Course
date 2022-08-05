using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Task15
{
    internal class Subtask2
    {
        public static void start()
        {
            List<string> strings = new List<string>();
            strings.Add("APLLE");
            strings.Add("COUNT");
            strings.Add("BARBECUE");
            strings.Add("ANSWER");

            var result = strings.Where(s => char.IsLetter(s[0]))
                .GroupBy(s => s[0])
                .Select(s => new
                {
                    Name = s.Key,
                    Count = s.Select(i => i.Length).Sum()
                })
                .OrderByDescending(s => s.Count)
                .ThenByDescending(s => s.Name);

            foreach(var i in result)
            {
                Console.WriteLine(i.Name + " - " + i.Count);
            }
        }
    }
}
