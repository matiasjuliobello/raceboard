using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceBoard.DTOs
{
    public class DateTimeRangeRequest
    {
        public DateTimeOffset Start {  get; set; }
        public DateTimeOffset End { get; set; }
    }
}
