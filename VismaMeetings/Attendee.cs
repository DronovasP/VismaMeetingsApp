using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaMeetings
{
    public class Attendee
    {
        public string Name { get; set; }
        public DateTime JoinTime { get; set; }
        public Attendee() { }
        public Attendee(string name, DateTime joinTime)
        {
            Name = name;
            JoinTime = joinTime;
        }
        public override string ToString()
        {
            return String.Format("{0} {1}", Name, JoinTime.ToString("yyyy/MM/dd H:mm"));
        }
    }
}
